#include "esp_log.h"
#include "nvs_flash.h"
#include "protocol_examples_common.h"

#include "rc522.h"
#include "b64.h"
#include "cutils.h"
#include "estr.h"
#include "cJSON.h"
#include "discord.h"
#include "discord/session.h"
#include "discord/message.h"

static const char* TAG = "discord-rfid";

typedef enum {
    PACKAGE_UNKNOWN,
    PACKAGE_TAG,
    PACKAGE_PING,
    PACKAGE_PONG
} package_type_t;

typedef struct {
    char* name;
    char* version;
    uint64_t uptime;
    uint64_t heap;
} device_t;

typedef struct {
    package_type_t type;
    device_t* device;
    uint64_t* serial_number;
} package_t;

static device_t* device_create();
static package_t* package_create_tag(uint64_t serial_number);
static char* package_to_string(package_t* package);
static void device_free(device_t* device);
static void package_free(package_t* package);

static discord_handle_t bot;

static void bot_event_handler(void* handler_arg, esp_event_base_t base, int32_t event_id, void* event_data) {
    discord_event_data_t* data = (discord_event_data_t*) event_data;

    switch(event_id) {
        case DISCORD_EVENT_CONNECTED: {
                discord_session_t* session = (discord_session_t*) data->ptr;
                discord_session_dump_log(ESP_LOGI, TAG, session);
                rc522_start2();
            }
            break;
        
        case DISCORD_EVENT_MESSAGE_RECEIVED: {
                discord_message_t* msg = (discord_message_t*) data->ptr;
                discord_message_dump_log(ESP_LOGI, TAG, msg);
            }
            break;

        case DISCORD_EVENT_RECONNECTING:
            ESP_LOGW(TAG, "Bot is reconnecting...");
            rc522_pause();
            break;
        
        case DISCORD_EVENT_DISCONNECTED:
            ESP_LOGW(TAG, "Bot logged out");
            rc522_destroy();
            break;
    }
}

void rfid_handler(uint8_t* sn) {
    discord_gateway_state_t state;
    discord_get_state(bot, &state);

    if(state != DISCORD_STATE_CONNECTED) {
        ESP_LOGW(TAG, "Discard RFID Tag. Not connected with Discord");
        return;
    }

    uint64_t tag64 = rc522_sn_to_u64(sn);

	ESP_LOGI(TAG, "Tag: %" PRIu64 " (%#x %#x %#x %#x %#x)",
		tag64, sn[0], sn[1], sn[2], sn[3], sn[4]
	);

    package_t* package = package_create_tag(tag64);
    char* pstr = package_to_string(package);
    
    ESP_LOGI(TAG, "%s", pstr);
    // send package...

    free(pstr);
    package_free(package);
}

void app_main(void) {
    ESP_ERROR_CHECK(nvs_flash_init());
    ESP_ERROR_CHECK(esp_netif_init());
    ESP_ERROR_CHECK(esp_event_loop_create_default());
    ESP_ERROR_CHECK(example_connect());

    discord_config_t cfg = {
        .intents = DISCORD_INTENT_GUILD_MESSAGES
    };

    bot = discord_create(&cfg);
    ESP_ERROR_CHECK(discord_register_events(bot, DISCORD_EVENT_ANY, bot_event_handler, NULL));

    ESP_ERROR_CHECK(rc522_init(&(rc522_config_t){
        .miso_io  = CONFIG_DISCORD_RFID_MISO,
        .mosi_io  = CONFIG_DISCORD_RFID_MOSI,
        .sck_io   = CONFIG_DISCORD_RFID_SCK,
        .sda_io   = CONFIG_DISCORD_RFID_SDA,
        .callback = &rfid_handler
    }));

    ESP_ERROR_CHECK(discord_login(bot));

#ifdef CONFIG_DISCORD_RFID_DEVELOPMENT
    uint32_t _heap = 0;

    while(true) {
        uint32_t heap = esp_get_free_heap_size();
        
        if(abs(_heap - heap) >= 125) {
            esp_log_write(ESP_LOG_NONE, TAG, "%s-debug: (heap=%d)\n", TAG, (_heap = heap));
        }

        vTaskDelay(35 / portTICK_PERIOD_MS);
    }
#endif
}

static device_t* device_create() {
    return cu_ctor(device_t,
        .name = strdup("ESP32"),
        .version = strdup(esp_get_idf_version()),
        .uptime = esp_timer_get_time(),
        .heap = esp_get_free_heap_size()
    );
}

static package_t* package_create_tag(uint64_t serial_number) {
    package_t* pckg = cu_ctor(package_t,
        .type = PACKAGE_TAG,
        .device = device_create(),
        .serial_number = malloc(sizeof(uint64_t))
    );
    
    *(pckg->serial_number) = serial_number;
    
    return pckg;
}

static char* package_to_string(package_t* package) {
    if(!package) {
        return NULL;
    }

    cJSON* pjson = cJSON_CreateObject();
    cJSON_AddItemToObject(pjson, "Type", cJSON_CreateNumber(package->type));

    if(package->serial_number) {
        cJSON_AddItemToObject(pjson, "SerialNumber", cJSON_CreateNumber(*(package->serial_number)));
    }

    if(package->device) {
        cJSON* djson = cJSON_CreateObject();
        cJSON_AddItemToObject(djson, "Name", cJSON_CreateString(package->device->name));
        cJSON_AddItemToObject(djson, "Version", cJSON_CreateString(package->device->version));
        cJSON_AddItemToObject(djson, "Uptime", cJSON_CreateNumber(package->device->uptime));
        cJSON_AddItemToObject(djson, "Heap", cJSON_CreateNumber(package->device->heap));
        cJSON_AddItemToObject(pjson, "Device", djson);
    }
    
    char* json = cJSON_PrintUnformatted(pjson);
    cJSON_Delete(pjson);

    char* result = (char*) b64_encode((unsigned char*) json, strlen(json), NULL);
    free(json);

    return result;
}

static void device_free(device_t* device) {
    if(!device) {
        return;
    }

    free(device->name);
    free(device->version);
    free(device);
}

static void package_free(package_t* package) {
    if(!package) {
        return;
    }

    free(package->serial_number);
    device_free(package->device);
    free(package);
}