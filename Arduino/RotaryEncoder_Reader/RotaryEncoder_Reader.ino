// Interrupt and Rotary encoder
// Arduino UNO has 2 interrupt pins, D2, D3
#include <PinChangeInt.h>
#define SERIAL_BAUD 9600
int encoderPin1 = 2; // interrupt 0
int encoderPin2 = 3; // interrupt 1

// rotary encoders
volatile long LastEncoded = 0;
volatile long encoderValue = 0;
float encoderOuput = 0;

void updateEncoder() {
  long MSB = digitalRead(encoderPin1); //MSB = most significant bit
  long LSB = digitalRead(encoderPin2); //LSB = least significant bit
  long encoded = (MSB << 1) | LSB; //converting the 2 pin value to single number
  long sum  = (LastEncoded << 2) | encoded; //adding it to the previous encoded value
  if(sum == 0b1101 || sum == 0b0100 || sum == 0b0010 || sum == 0b1011) encoderValue --;
  if(sum == 0b1110 || sum == 0b0111 || sum == 0b0001 || sum == 0b1000) encoderValue ++;
  LastEncoded = encoded; //store this value for next time
}

void setup()                         
{
  Serial.begin(SERIAL_BAUD);

  // rotary encoder setup
  pinMode(encoderPin1, INPUT); 
  pinMode(encoderPin2, INPUT);
  digitalWrite(encoderPin1, HIGH); //turn pullup resistor on
  digitalWrite(encoderPin2, HIGH); //turn pullup resistor on
  //call updateEncoder() when any high/low changed seen
  attachInterrupt(digitalPinToInterrupt(encoderPin1), updateEncoder, CHANGE); 
  attachInterrupt(digitalPinToInterrupt(encoderPin2), updateEncoder, CHANGE);
}
void loop() 
{
//  Uncomment below to check values of PID  
  Serial.println(encoderOuput);
  encoderOuput = encoderValue;
  delay(50);
}



