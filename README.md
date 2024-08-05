# 7DTD Server Announcer

The 7DTD Server Announcer is a C# application that connects to a local 7 Days to Die server via telnet and periodically sends messages to all players using the "say" command. This application reads its configuration from `config.txt`, allowing customization of server connection details, message source, and message frequency.

## Features

- **Telnet Connection:** Connects to a 7 Days to Die server via telnet protocol.
- **Configuration File:** Reads server connection details (IP, port, password), message file location, and frequency from `config.txt`.
- **Message Sending:** Sends random messages from `messages.txt` to the server at specified intervals.
- **Console Status:** Displays status messages in the console indicating successful or failed message transmissions.
- **Graceful Termination:** Handles Ctrl+C to exit gracefully, closing connections and cleaning up resources.

## Setup

1. **Prerequisites:**
   - Ensure `.NET Framework` or `.NET Core` runtime is installed on your system.

2. **Configuration:**
   - Place `config.txt` and `messages.txt` in the same directory as `7DTDServerAnnouncer.exe`.
   - Edit `config.txt` with your server IP, port, password, message file name, and frequency settings.

3. **Execution:**
   - Run `7DTDServerAnnouncer.exe`.
   - Monitor the console output for status updates on message sending.

## Configuration File (`config.txt`)

Example `config.txt` format:

```plaintext
ServerIP=127.0.0.1
Port=8081
Password=myserverpassword
MessageFileName=messages.txt
Frequency=30
