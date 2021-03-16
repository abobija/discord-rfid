#include "esp_log.h"

#include "rc522.h"
#include "discord.h"

static const char* TAG = "discord-rfid";

void rfid_handler(uint8_t* sn) {
	ESP_LOGI(TAG, "Tag: %#x %#x %#x %#x %#x", 
		sn[0], sn[1], sn[2], sn[3], sn[4]
	);
}

void app_main(void) {
    const rc522_start_args_t start_args = {
        .miso_io  = CONFIG_DISCORD_RFID_MISO,
        .mosi_io  = CONFIG_DISCORD_RFID_MOSI,
        .sck_io   = CONFIG_DISCORD_RFID_SCK,
        .sda_io   = CONFIG_DISCORD_RFID_SDA,
        .callback = &rfid_handler
    };

    rc522_start(start_args);
}