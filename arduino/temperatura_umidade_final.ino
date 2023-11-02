#include <DHT.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClientSecure.h>
#include <WiFiClient.h>

// variáveis para sensor THT 11
const byte dhtPin = 2;
#define DHTTYPE DHT11
DHT dht(dhtPin, DHTTYPE);

float temperature, humidity;
char buff_temperature[6];
char buff_humidity[6];

String temperature_string;
String humidity_string;

// variáveis para conexão wifi e API
const char* ssid = "<Usuário Wifi>";
const char* password = "<Senha do Wifi>";

//API Temperatura / Humidade
String serverTemperature = "https://<servidor-api>/api/sensor/temperatura?code=<TOKEN_FUNCTION>==";
String serverHumidity = "https://<servidor-api>/api/sensor/humidade?code=<TOKEN_FUNCTION>==";

// the following variables are unsigned longs because the time, measured in
// milliseconds, will quickly become a bigger number than can be stored in an int.
// Timer set to 10 minutes (600000)
//unsigned long timerDelay = 600000;
//Set timer to 5 seconds (5000)
unsigned long timerDelay = 5000;

void setup() {
  // Pino do sensor
  dht.begin();
  Serial.begin(9600);
  delay(100);

  // Inicialização do wi-fi
  WiFi.persistent(false);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  Serial.println("Connecting");
  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
 
  Serial.println("Timer set to 5 seconds (timerDelay variable), it will take 5 seconds before publishing the first reading.");
}

void loop() {
  // Valores de temperatura e humidade:
  if (readSensor()) {
    Serial.print("Temperatura  = ");
    Serial.println(temperature_string);
    Serial.print("Humidade  = ");
    Serial.println(humidity_string);

    uploadData(serverTemperature, "temperatura", temperature_string);
    delay(500);
    uploadData(serverHumidity, "humidade", humidity_string);
    delay(500);
  }
}

// Leitura do sensor THT 11
boolean readSensor() {
  static unsigned long timer = 0;
  unsigned long interval = 60000; // 30 segundos

  if (millis() - timer > interval) {
    timer = millis();   
    humidity = dht.readHumidity();
    temperature = dht.readTemperature();

    // conversão para string
    dtostrf(temperature,5, 2, buff_temperature);
    temperature_string = String(buff_temperature);
    dtostrf(humidity,5, 2, buff_humidity);
    humidity_string = String(buff_humidity);
    
    if (isnan(humidity) || isnan(temperature)) {
      Serial.println("Failed to read from DHT sensor!");      
      return false;
    }
    return true;
  } else {
    return false;
  }
}

void uploadData(String serverPath, String key, String value) {
  //Check WiFi connection status
    if(WiFi.status()== WL_CONNECTED){
      //WiFiClient client;
      HTTPClient http;

      WiFiClientSecure *client = new WiFiClientSecure;
      client->setInsecure();

      // Your Domain name with URL path or IP address with path
      http.begin(*client, serverPath.c_str());
  
      // If you need Node-RED/server authentication, insert user and password below
      http.addHeader("x-token", "<TOKEN>");
      http.addHeader("Content-Type", "application/json");
      String jsonBody = "{ \"" + key + "\": " + value + " }";

      Serial.print(jsonBody);

      int httpResponseCode = http.POST(jsonBody);
      //int httpResponseCode = http.GET();
      
      if (httpResponseCode>0) {
        Serial.print("HTTP Response code: ");
        Serial.println(httpResponseCode);
        String payload = http.getString();
        Serial.println(payload);
      } else {
        Serial.print("Error code: ");
        Serial.println(httpResponseCode);
        Serial.printf("[HTTPS] POST... failed, error: %s\n", http.errorToString(httpResponseCode).c_str());
      }
      // Free resources
      http.end();
      delete client;
      client = nullptr;
  } else {
      Serial.println("WiFi Disconnected");
  }
}
