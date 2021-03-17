#include "esp_log.h"
#include "nvs_flash.h"
#include "protocol_examples_common.h"

#include "rc522.h"
#include "discord.h"
#include "discord/session.h"
#include "discord/message.h"

static const char* TAG = "discord-rfid";

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
	ESP_LOGI(TAG, "Tag: %#x %#x %#x %#x %#x", 
		sn[0], sn[1], sn[2], sn[3], sn[4]
	);
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