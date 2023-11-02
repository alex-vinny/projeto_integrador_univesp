#include <DHT.h>

const byte dhtPin = 5;
#define DHTTYPE DHT22
DHT dht(dhtPin, DHTTYPE);

float temperature, temperature_fahrenheit, humidity;

void setup() {
  dht.begin();
  Serial.begin(115200);
}

void loop() {
  if (readSensor()) {
    Serial.print("Temperatura  = ");
    Serial.print(temperature_fahrenheit);
    Serial.println(" Fahrenheit");
    Serial.print("Humidade  = ");
    Serial.print(humidity);
    Serial.println(" %RH");
  }
}

boolean readSensor() {
  static unsigned long timer = 0;
  unsigned long interval = 5000;

  if (millis() - timer > interval) {
    timer = millis();   
    humidity = dht.readHumidity();
    temperature = dht.readTemperature();
    temperature_fahrenheit =  dht.readTemperature(true);

    if (isnan(humidity) || isnan(temperature) || isnan(temperature_fahrenheit)) {
      Serial.println("Failed to read from DHT sensor!");      
      return false;
    }
    return true;
  } else {
    return false;
  }
}