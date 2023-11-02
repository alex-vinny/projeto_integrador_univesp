#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClientSecure.h>
#include <WiFiClient.h>

int pinoAnalogico = 13; // A0 Analógico
int pinoDigital = 12; // Digital do Sensor 

int rele = 14; // Digital Relé

int estadoSensor = 0;
int ultimoEstSensor = 0;

int  valAnalogIn; // Valor analógico no código

// variáveis para conexão wifi e API
const char* ssid = "<USUÁRIO WIFI>";
const char* password = "<SENHA WIFI>";

//API Temperatura / Humidade
String server = "https://<servidor-api>/api/sensor/irrigacao?code=<TOKENFUNCTION>==";

void setup() {

  Serial.begin(9600);
  delay(100);
  pinMode(rele, OUTPUT); // Declara o Rele como Saída Digital 

  pinMode(pinoDigital, INPUT);

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
  valAnalogIn = analogRead(0);
  int porcento = map(valAnalogIn, 1023, 0, 0, 100); // Traforma o valor analógico em porcentagem

  Serial.print("Umidade: "); // Imprime o símbolo no valor
  Serial.print(valAnalogIn);
  Serial.print(", ");
  Serial.print(porcento); // Imprime o valor em Porcentagem no monitor Serial
  Serial.println("%");

  if (porcento <= 76) { // Se a porcentagem for menor ou igual à 76%. OBS: Você pode alterar essa porcentagem
  
    Serial.println("Bomba ligada"); // Imprime no monitor serial
    digitalWrite(rele, HIGH); // Desliga Relé    
  } else { // Caso contrario   
    Serial.println("Bomba desligada"); // Imprime a no monitor serial
    digitalWrite(rele, LOW); // Aciona Relé
  }

  delay (5000);
  String strPorcento = String(porcento);
  bool abrirBomba = porcento <= 76;
  Serial.print("% : ");
  Serial.println(strPorcento);
  Serial.print("Bomba: ");
  Serial.println(abrirBomba);
  uploadData(server, strPorcento, abrirBomba);
  delay (5000);
}

void uploadData(String serverPath, String value, bool abriu) {
  //Check WiFi connection status
    if(WiFi.status()== WL_CONNECTED){
      //WiFiClient client;
      HTTPClient http;

      WiFiClientSecure *client = new WiFiClientSecure;
      client->setInsecure();

      // Your Domain name with URL path or IP address with path
      http.begin(*client, serverPath.c_str());
  
      // If you need Node-RED/server authentication, insert user and password below
      http.addHeader("x-token", "<TOKEN_API>");
      http.addHeader("Content-Type", "application/json");
      String jsonBody = "{ \"humidade\": " + value + ", \"abriubomba\": \"" + (abriu ? "true" : "false") + "\" }";

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
