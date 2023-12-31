#define BLYNK_PRINT Serial
#define BLYNK_TEMPLATE_ID "ID do template"
#define BLYNK_DEVICE_NAME "Nome do projeto"

// Biblioteca necessárias 

#include <ESP8266WiFi.h>
#include <BlynkSimpleEsp8266.h>
#include <DHT.h> 


char auth[] = "Token"; // Aqui é necessário inserir o token mostrado no site

char ssid[] = "nome da rede";   // Insira o nome da rede Wi-fi utilizada 
char pass[] = "senha da rede "; // Insira a senha da rede Wi-fi utilizada 


#define DHTPIN 2  // Aqui é o pino digital que estamos utilizando, no nosso caso D4 (GPIO 2)

#define DHTTYPE DHT11     // Declarando o sensor 


DHT dht(DHTPIN, DHTTYPE);
BlynkTimer timer;

//Esta função envia o tempo de atividade do Arduino a cada segundo para o Pino Vitual (5).
// você também pode definer com que frequência enviar dados para o aplicativo Blynk.

void sendSensor() {

float h = dht.readHumidity(); 
float t = dht.readTemperature();

  if (isnan(h) || isnan(t)) {
    Serial.println("Failed to read from DHT sensor!");
    return;
  }
 
  Blynk.virtualWrite(V5, h); // Pino Virtual 5 para umidade
  Blynk.virtualWrite(V6, t); // Pino Virtual 6 para temperatura
}

void setup()
{
  Serial.begin(9600);

  Blynk.begin(auth, ssid, pass);

  dht.begin();

  timer.setInterval(1000L, sendSensor);
}
void loop()
{
  Blynk.run();
  timer.run();
}
