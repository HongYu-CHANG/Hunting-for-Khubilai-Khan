#define SWITCH 2
#define SPEED 3

// States
#define CYCLELENGTH 10000
#define SWITCHING 1
#define SPEEDUP 2
int state;
int cycles;

// Air pressure chip
#include <SFE_BMP180.h>
#include <Wire.h>
#define THRESHOLD 20
double iniPressure;
bool initial = false;
int speedState;

// Constructors
SFE_BMP180 pressure;
bool onOff;

void overallControl(char input);
double pressureMeasure();

void setup() {
  // Set pin modes
  pinMode(SWITCH, OUTPUT);
  pinMode(SPEED, OUTPUT);

  // Reset parameter
  state = 0;
  cycles = 0;
  iniPressure = 0;
  speedState = 0;
  onOff = false;

  // Enable air pressure chip
//  if (pressure.begin()){
//    delay(1);
//    //Serial.println("BMP180 init success");
//  }
//  else
//  {
//    //Serial.println("BMP180 init fail\n\n");
//    while(1); // Pause forever.
//  }
  
  // Message
  Serial.begin(9600);
//  Serial.println("");
//  Serial.println("Instructions:");
//  Serial.println("Enter 0 to switch on.");
//  Serial.println("Enter 1 to speed up.");
//  Serial.println("-----------------------------------");
//  Serial.println("");
}

void loop() {
  char input;
//  if(!initial){
//    iniPressure = pressureMeasure();
//    initial = true;
//  }
  
  while(Serial.available()){
    input = Serial.read();
  }
  
  // Use control signal to control switch on or speed up
  overallControl(input); 

  //Serial.println(speedState);
  //delay(50);
}

void overallControl(char input){

  double curPressure;

//  curPressure = pressureMeasure();
//  if(curPressure - iniPressure >= THRESHOLD){
//    input = '1';
//  }
  
  switch(input){
      case '0':
        state = SWITCHING;
//        Serial.print("Switching on......");
        break;
      case '1':
        state = SPEEDUP;
//        Serial.print("Speeding  up......");
        break;
  }
  
  switch(state){
    case SWITCHING:
      if(cycles < CYCLELENGTH){
//        if((cycles % 20) == 0){
//          Serial.print(".");
//        }
        digitalWrite(SWITCH, HIGH);
        cycles++;
      }
      else{
        digitalWrite(SWITCH, LOW);
//        Serial.println("  Switched on.");
        if(!onOff){
          speedState = 1;
          onOff = true;
        }
        else{
          speedState = 0;
          onOff = false;
        }
        cycles = 0;
        state = 0;
        Serial.println(speedState);
      }
      break;
    case SPEEDUP:
      if(onOff){
        if(cycles < CYCLELENGTH){
//          if((cycles % 20) == 0){
//            Serial.print(".");
//          }
          digitalWrite(SPEED, HIGH);
          cycles++;
        }
        else{
          digitalWrite(SPEED, LOW);
//          Serial.println("  Speed up done.");
          if(speedState < 4){
            speedState++;
          }
          else{
            speedState = 1;
          }
          cycles = 0;
          state = 0;
          Serial.println(speedState);
        }
        break;
    }
  }
}

//double pressureMeasure(){
//  double T,P,p0,a;
//
//  pressure.startPressure(3);
//  pressure.getPressure(P,T);
//
//  return P;
//  // Print out the measurement:
////  Serial.print("absolute pressure: ");
////  Serial.print(P,2);
////  Serial.print(" mb, ");
////  Serial.print(P*0.0295333727,2);
////  Serial.println(" inHg");
//}

