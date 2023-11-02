int PinoAnalogico = 13; // Define o pino 13 como Pino Analógico do sensor
int PinoDigital = 12; // Define pino D14 como Pino Digital do Sensor 

int Rele = 14; // Pino Digital D1 como Relé

int EstadoSensor = 0;
int UltimoEstSensor = 0;

int  ValAnalogIn; // Valor analógico no código

void setup() {

Serial.begin(9600); 
pinMode(Rele, OUTPUT); // Declara o Rele como Saída Digital 

pinMode(PinoDigital, INPUT);
}
void loop() {


ValAnalogIn = analogRead(PinoAnalogico);
int Porcento = map(ValAnalogIn, 1023, 0, 0, 100); // Traforma o valor analógico em porcentagem

Serial.print("Umidade: "); // Imprime o símbolo no valor
Serial.print(Porcento); // Imprime o valor em Porcentagem no monitor Serial
Serial.println("%");

if (Porcento <= 76) { // Se a porcentagem for menor ou igual à 76%. OBS: Você pode alterar essa porcentagem
 
Serial.println("Irrigando Planta"); // Imprime no monitor serial
digitalWrite(Rele, LOW); // Aciona Relé

}else { // Caso contrario 
 
Serial.println("Planta Irrigada"); // Imprime a no monitor serial
digitalWrite(Rele, HIGH); // Desliga Relé

delay (1000);
}
}
