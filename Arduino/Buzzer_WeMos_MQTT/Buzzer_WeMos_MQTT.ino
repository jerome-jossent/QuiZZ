//Compiler avec NodeMCU 1.0(ESP-12E Module)
//port série en 9600
//fils bouton sur D1 & GND

#define NAME "Jaune 1"

#include <ESP8266WiFi.h>
#include <Ticker.h>
#include <AsyncMqttClient.h> //https://github.com/marvinroger/async-mqtt-client

#define WIFI_SSID "Freebox-3ECC2D" //"REPLACE_WITH_YOUR_SSID"
#define WIFI_PASSWORD "movendos*!-tauricam-quintill%?-citer" //"REPLACE_WITH_YOUR_PASSWORD"
// Raspberry Pi Mosquitto MQTT Broker
#define MQTT_HOST IPAddress(192, 168, 1, 30)
// For a cloud MQTT broker, type the domain name
//#define MQTT_HOST "example.com"
#define MQTT_PORT 1883

#define BAUDS 9600

#define MQTT_PUB_TOPIC0 "buzzer/NewPlayer"  
#define MQTT_PUB_TOPIC1 "buzzer/tempo"
#define MQTT_PUB_TOPIC2 "buzzer/BUZZ"

char msg_buz_ON[30];
char msg_buz_OFF[30];
char buf[30];

AsyncMqttClient mqttClient;
Ticker mqttReconnectTimer;
WiFiEventHandler wifiConnectHandler;
WiFiEventHandler wifiDisconnectHandler;
Ticker wifiReconnectTimer;
unsigned long previousMillis;   // Stores last time
const long interval = 10000;        // Interval at which to publish sensor readings

#define PIN_BUZZ 5 //D1
int btn_state_prev;

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
  Serial.println("Connected !");
  Serial.print("Session present : ");
  Serial.println(sessionPresent);

  NewPlayer();
}
void onMqttDisconnect(AsyncMqttClientDisconnectReason reason) {
  Serial.println("Disconnected from MQTT.");
  if (WiFi.isConnected()) {
    mqttReconnectTimer.once(2, connectToMqtt);
  }
}
//void onMqttPublish(uint16_t packetId) {
//   Serial.print("Publish acknowledged. packetId: ");
//   Serial.println(packetId);
// }

void setup() {
  Serial.begin(BAUDS);
  Serial.println("*************BUZZER***************");
  
  pinMode(LED_BUILTIN, OUTPUT);     // Initialize the LED_BUILTIN pin as an output
  pinMode(PIN_BUZZ, INPUT_PULLUP);
  
  wifiConnectHandler = WiFi.onStationModeGotIP(onWifiConnect);
  wifiDisconnectHandler = WiFi.onStationModeDisconnected(onWifiDisconnect);
  
  mqttClient.onConnect(onMqttConnect);
  mqttClient.onDisconnect(onMqttDisconnect);
  
  //mqttClient.onSubscribe(onMqttSubscribe);
  //mqttClient.onUnsubscribe(onMqttUnsubscribe);
  //mqttClient.onPublish(onMqttPublish);
  
  mqttClient.setServer(MQTT_HOST, MQTT_PORT);
  // If your broker requires authentication (username and password), set them below
  //mqttClient.setCredentials("REPlACE_WITH_YOUR_USER", "REPLACE_WITH_YOUR_PASSWORD");
  connectToWifi();

  previousMillis = millis();
  btn_state_prev = digitalRead(PIN_BUZZ);
  Serial.println("setup ok"); 
}


void Publish(char* topic, char* value, bool retain = false, uint qos = 2)
{
  // qos (0) At most once, (1) At least once, (2) Exactly once.  
  uint16_t packetIdPub1 = mqttClient.publish(topic, qos, retain, value);
  Serial.printf("Topic \"%s\" at QoS %i, packetId: %i Message: \"%s\" \n", 
                topic, qos, packetIdPub1, value);
}

void loop(){
  
  int btn_state = digitalRead(PIN_BUZZ);
  
  if (btn_state != btn_state_prev)
  { 
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillis >= 20)
    {
      btn_state_prev = btn_state;
      previousMillis = currentMillis;
      
      if (btn_state){
        Serial.println("unpushed");
        BUZZ_Unpushed();
      }else{
        Serial.println("pushed");
        BUZZ_Pushed();
      }
    }
  }
}

void NewPlayer(){
  //Subscribe()
  Publish(MQTT_PUB_TOPIC0, NAME, false, 1);

  String s = String(NAME) + " ON";
  s.toCharArray(msg_buz_ON, s.length() + 1);
  
  s = String(NAME) + " OFF";
  s.toCharArray(msg_buz_OFF, s.length() + 1);
  
  delay(100);
}

void BUZZ_Pushed(){
  Publish(MQTT_PUB_TOPIC2, msg_buz_ON);  
  digitalWrite(LED_BUILTIN, LOW);
}

void BUZZ_Unpushed(){
  Publish(MQTT_PUB_TOPIC2, msg_buz_OFF);  
  digitalWrite(LED_BUILTIN, HIGH);
}