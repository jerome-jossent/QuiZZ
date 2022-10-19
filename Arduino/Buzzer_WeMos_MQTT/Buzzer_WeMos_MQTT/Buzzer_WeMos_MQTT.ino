#include <ESP8266WiFi.h>
#include <Ticker.h>
#include <AsyncMqttClient.h>
#include "OneButton.h"

#define WIFI_SSID "Freebox-3ECC2D" //"REPLACE_WITH_YOUR_SSID"
#define WIFI_PASSWORD "movendos*!-tauricam-quintill%?-citer" //"REPLACE_WITH_YOUR_PASSWORD"
// Raspberry Pi Mosquitto MQTT Broker
#define MQTT_HOST IPAddress(192, 168, 1, 30)
// For a cloud MQTT broker, type the domain name
//#define MQTT_HOST "example.com"
#define MQTT_PORT 1883

#define NAME "Jaune 1"
#define BAUDS 9600

#define MQTT_PUB_TOPIC0 "buzzer/NewPlayer"  
#define MQTT_PUB_TOPIC1 "buzzer/tempo"
#define MQTT_PUB_TOPIC2 "buzzer/BUZZ"

AsyncMqttClient mqttClient;
Ticker mqttReconnectTimer;
WiFiEventHandler wifiConnectHandler;
WiFiEventHandler wifiDisconnectHandler;
Ticker wifiReconnectTimer;
unsigned long previousMillis = 0;   // Stores last time
const long interval = 10000;        // Interval at which to publish sensor readings

#define PIN_BUZZ 5
OneButton monBouton(PIN_BUZZ,
                    false       , // bouton à l'état HIGH si appuyé
                    false         // résistance de tirage pull-up désactivée
                   );

void connectToWifi() {
  Serial.println("Connecting to Wi-Fi...");
  WiFi.begin(WIFI_SSID, WIFI_PASSWORD);
}
void onWifiConnect(const WiFiEventStationModeGotIP& event) {
  Serial.println("Connected to Wi-Fi.");
  connectToMqtt();
}
void onWifiDisconnect(const WiFiEventStationModeDisconnected& event) {
  Serial.println("Disconnected from Wi-Fi.");
  mqttReconnectTimer.detach(); // ensure we don't reconnect to MQTT while reconnecting to Wi-Fi
  wifiReconnectTimer.once(2, connectToWifi);
}
void connectToMqtt() {
  Serial.println("Connecting to MQTT...");
  mqttClient.connect();
}
void onMqttConnect(bool sessionPresent) {
  Serial.println("Connected to MQTT.");
  Serial.print("Session present: ");
  Serial.println(sessionPresent);
}
void onMqttDisconnect(AsyncMqttClientDisconnectReason reason) {
  Serial.println("Disconnected from MQTT.");
  if (WiFi.isConnected()) {
    mqttReconnectTimer.once(2, connectToMqtt);
  }
}void onMqttPublish(uint16_t packetId) {
  Serial.print("Publish acknowledged.");
  Serial.print("  packetId: ");
  Serial.println(packetId);
}
void setup() {
  Serial.begin(BAUDS);
  Serial.println();
  
  pinMode(LED_BUILTIN, OUTPUT);     // Initialize the LED_BUILTIN pin as an output
  monBouton.attachClick(BUZZ); // liaison de l'évènement avec la fonction doubleclic
  monBouton.setDebounceTicks(20); // pause antirebonds 20 ms
  
  wifiConnectHandler = WiFi.onStationModeGotIP(onWifiConnect);
  wifiDisconnectHandler = WiFi.onStationModeDisconnected(onWifiDisconnect);
  
  mqttClient.onConnect(onMqttConnect);
  mqttClient.onDisconnect(onMqttDisconnect);
  //mqttClient.onSubscribe(onMqttSubscribe);
  //mqttClient.onUnsubscribe(onMqttUnsubscribe);
  mqttClient.onPublish(onMqttPublish);
  mqttClient.setServer(MQTT_HOST, MQTT_PORT);
  // If your broker requires authentication (username and password), set them below
  //mqttClient.setCredentials("REPlACE_WITH_YOUR_USER", "REPLACE_WITH_YOUR_PASSWORD");
  connectToWifi();


  delay(100);
 // NewPlayer();  
}

void NewPlayer(){
    uint16_t packetIdPub1 = mqttClient.publish(MQTT_PUB_TOPIC0, 1, true, NAME);
    Serial.printf("Publishing on topic %s at QoS 1, packetId: %i", MQTT_PUB_TOPIC0, packetIdPub1);
    Serial.printf("Message: %.2f n", NAME);
    delay(100);
}

void loop() {
  unsigned long currentMillis = millis();
  // Every X number of seconds (interval = 10 seconds) 
  // it publishes a new MQTT message
  if (currentMillis - previousMillis >= interval) {
    // Save the last time a new reading was published
    previousMillis = currentMillis;

    // Publish an MQTT message on topic
    uint16_t packetIdPub1 = mqttClient.publish(MQTT_PUB_TOPIC1, 1, true, String(currentMillis).c_str());
    Serial.printf("Publishing on topic %s at QoS 1, packetId: %i", MQTT_PUB_TOPIC1, packetIdPub1);
    Serial.printf("Message: %.2f n", currentMillis);
  }

  int sensorValue = digitalRead(PIN_BUZZ);
  if (sensorValue == LOW) {
    uint16_t packetIdPub1 = mqttClient.publish(MQTT_PUB_TOPIC2, 1, true, "coucou");
    Serial.printf("Publishing on topic %s at QoS 1, packetId: %i", MQTT_PUB_TOPIC2, packetIdPub1);    
    digitalWrite(LED_BUILTIN, LOW);
  } else {    
    digitalWrite(LED_BUILTIN, HIGH);
  }
  delay(10);
}

void BUZZ(){
  uint16_t packetIdPub1 = mqttClient.publish(MQTT_PUB_TOPIC2, 1, true, "coucou");
  Serial.printf("Publishing on topic %s at QoS 1, packetId: %i", MQTT_PUB_TOPIC2, packetIdPub1);    
  digitalWrite(LED_BUILTIN, LOW);
}