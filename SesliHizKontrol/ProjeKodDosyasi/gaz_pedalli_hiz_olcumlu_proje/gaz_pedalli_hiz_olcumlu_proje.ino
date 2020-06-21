#include "Arduino.h"
#include "SoftwareSerial.h"

#define motor1ileri 4
#define motor1geri 7
#define motor1hiz 5

#define motor2ileri 9
#define motor2geri 10
#define motor2hiz 11

unsigned long zaman = 0;
unsigned long now;
unsigned long last;
unsigned long sifirZaman = 0;

int sayac = 0;
float rpm;
float mpm;
float kmh;
bool birKereGor = false;
bool sifirOlduMu = true;
bool deger;
bool gordu;

void setup() {
  Serial.begin(9600);
  pinMode(motor1ileri, OUTPUT);
  pinMode(motor1geri, OUTPUT);
  pinMode(motor1hiz, OUTPUT);
  pinMode(motor2ileri, OUTPUT);
  pinMode(motor2geri, OUTPUT);
  pinMode(motor2hiz, OUTPUT);
  pinMode(8, INPUT);
  digitalWrite(motor1ileri, HIGH);
  digitalWrite(motor1geri, LOW);
  digitalWrite(motor2ileri, HIGH);
  digitalWrite(motor2geri, LOW);
}

int pot;
void loop() {
  pot = analogRead(A0);

  analogWrite(motor1hiz, map(pot, 0, 1023, 0, 255));
  analogWrite(motor2hiz, map(pot, 0, 1023, 0, 255));

  gordu = digitalRead(8);
  zaman = millis();
  if (gordu) {
    if (sifirOlduMu) {
      sifirOlduMu = false;
      sifirZaman = zaman;
      sayac++;
      if (sayac == 20) {
        last = now;
        now = zaman;
        rpm = 60000 / (now - last);
        Serial.print("RPM:");
        Serial.println(rpm);
        mpm = ((rpm * (4.5 * 2 * 3.14)) / 100);
        kmh = (mpm / 1000) * 60;
        Serial.print("KMH:");
        Serial.println(kmh);
        sayac = 0;
      }
    }
  }
  else if (!gordu) {
    sifirOlduMu = true;
  }
  if (zaman - sifirZaman > 250) {
    sifirZaman = zaman;
    Serial.println("KMH:0.00");
    Serial.println("RPM:0.00");
    last = now;
    now = zaman;
  }


}
