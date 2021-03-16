# :robot: ESP32 RFID Discord Bot

This is ESP-IDF application that represent ESP32 RFID Discord Bot, and it is a part of [discord-rfid](https://github.com/abobija/discord-rfid) repository.

## Build

Generate Discord certificates with next command:

```
./components/esp-discord/certgen.sh
```

And then build the project:

```
idf.py build
```

## Components

Project uses and depends upon next components:

- [esp-discord](https://github.com/abobija/esp-discord)
- [esp-idf-rc522](https://github.com/abobija/esp-idf-rc522)

## Wiring

Connect MFRC522 module with ESP32 according to next table:

| ESP32         | MFRC522       |
| ------------- | ------------- |
| GPIO25        | MISO          |
| GPIO23        | MOSI          |
| GPIO19        | SCK           |
| GPIO22        | SDA           |

All of above GPIO pinout is configurable (via menuconfig - `idf.py menuconfig`) in `Discord RFID` menu.

## Author

GitHub: [abobija](https://github.com/abobija)<br>
Homepage: [abobija.com](https://abobija.com)

## License

[MIT](LICENSE)