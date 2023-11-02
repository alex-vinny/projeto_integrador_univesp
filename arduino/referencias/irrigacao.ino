int pinoAnalogico = 13; // Define o pino 13 como Pino Analógico do sensor
int pinoDigital = 12; // Define pino D14 como Pino Digital do Sensor 

int rele = 14; // Pino Digital D1 como Relé

int estadoSensor = 0;
int ultimoEstSensor = 0;

int  valAnalogIn; // Valor analógico no código

void setup() {

  Serial.begin(9600); 
  pinMode(rele, OUTPUT); // Declara o Rele como Saída Digital 

  pinMode(pinoDigital, INPUT);
}

void loop() {
  valAnalogIn = analogRead(pinoAnalogico);
  int porcento = map(valAnalogIn, 1023, 0, 0, 100); // Traforma o valor analógico em porcentagem

  Serial.print("Umidade: "); // Imprime o símbolo no valor
  Serial.print(porcento); // Imprime o valor em Porcentagem no monitor Serial
  Serial.println("%");

  if (porcento <= 76) { // Se a porcentagem for menor ou igual à 76%. OBS: Você pode alterar essa porcentagem
  
    Serial.println("Irrigando Planta"); // Imprime no monitor serial
    digitalWrite(rele, LOW); // Aciona Relé

  } else { // Caso contrario 
  
    Serial.println("Planta Irrigada"); // Imprime a no monitor serial
    digitalWrite(rele, HIGH); // Desliga Relé

    delay (1000);
  }
}
