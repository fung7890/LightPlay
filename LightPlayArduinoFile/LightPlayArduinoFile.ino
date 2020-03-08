#include <Adafruit_NeoPixel.h>
#define LED_PIN    6
#define LED_COUNT 300

Adafruit_NeoPixel strip(LED_COUNT, LED_PIN, NEO_GRB + NEO_KHZ800);

bool on = true;
int data;
int startLED = 37;
int totalLED = 103; // starts at 0
int checkPos, checkLEDToAdd;

void setup() {
  Serial.begin(9600); // connect
  strip.begin();
  strip.show(); // initialize to all off
  strip.setBrightness(200);

  
//  for (int i=0; i<104; ++i) { // 38 start from arduino
//    arr1[i] = i + 38;
//  }  
}

//void turnOnLEDGroup(int pos[], int sizeOfPos, int delayTime, int c1, int c2, int c3) {
//  
//  for (int i=0; i<sizeOfPos; ++i) {
//    strip.setPixelColor(pos[i], c1, c2, c3); // set colour for that pos
//  }
//
//  strip.setPixelColor(pos, c1, c2, c3);
//  strip.show();
//  
//  delay(delayTime);
//  
//  for (int i=0; i<sizeOfPos; ++i) {
//    strip.setPixelColor(pos[i], 0, 0, 0); // set off for that pos
//  }
//  strip.show();
//}
void turnColorOn(int c1, int c2, int c3) {
  strip.clear();
  strip.show(); // initialize to all off
  for (int i=38; i<140; ++i) {
    strip.setPixelColor(i, c1, c2, c3);
  }
  strip.show();
}

void turnColorOff() {
  strip.clear();
  strip.show(); // initialize to all off
}

void turnOnLED(int pos, int delayTime, int c1, int c2, int c3, int ledToAdd) {
  int posRight, posLeft;

  if (pos != checkPos || ledToAdd != checkLEDToAdd) {
    strip.clear();
    strip.show(); // initialize to all off
    
    if (pos == totalLED) {
      posRight = 0;
      posLeft = totalLED - 1;
    } else if (pos == 0) {
      posRight = 1;
      posLeft = totalLED;
    } else {
      posRight = pos + 1;
      posLeft = pos - 1;
    }
  
    checkPos = pos; // keep track of pos so we won't have to re-run if pos is same
    checkLEDToAdd = ledToAdd; // keep track of ledToAdd so we re-run if ledToAdd is different
    
    pos += startLED;
    posRight += startLED;
    posLeft += startLED;
  
    strip.setPixelColor(pos, c1, c2, c3);
    strip.setPixelColor(posRight, c1, c2, c3);
    strip.setPixelColor(posLeft, c1, c2, c3);

    if (ledToAdd > 1) { // base of 3 leds
      if (ledToAdd >= 2) { // 1 extra on each side
        strip.setPixelColor(posLeft - 1, c1, c2, c3);
        strip.setPixelColor(posRight + 1, c1, c2, c3);    
        strip.setPixelColor(posLeft - 2, c1, c2, c3);
        strip.setPixelColor(posRight + 2, c1, c2, c3);   
       
        if (ledToAdd >= 3) { // 2 extra on each side
          strip.setPixelColor(posLeft - 3, c1, c2, c3);
          strip.setPixelColor(posRight + 3, c1, c2, c3);    
          strip.setPixelColor(posLeft - 4, c1, c2, c3);
          strip.setPixelColor(posRight + 4, c1, c2, c3);  
          strip.setPixelColor(posLeft - 5, c1, c2, c3);
          strip.setPixelColor(posRight + 5, c1, c2, c3);    
        
          if (ledToAdd >= 4) { // 3 extra on each side
          strip.setPixelColor(posLeft - 6, c1, c2, c3);
          strip.setPixelColor(posRight + 6, c1, c2, c3);    
          strip.setPixelColor(posLeft - 7, c1, c2, c3);
          strip.setPixelColor(posRight + 7, c1, c2, c3);  
          strip.setPixelColor(posLeft - 8, c1, c2, c3);
          strip.setPixelColor(posRight + 8, c1, c2, c3);    
          strip.setPixelColor(posLeft - 9, c1, c2, c3);
          strip.setPixelColor(posRight + 9, c1, c2, c3);   
          }          
        }
      }
    }
    strip.show();  
  }
//  Serial.flush();

}

void loop() {

  if (Serial.available()) {
    char receivedPos[] = {' ', ' ', ' ',' ', ' ', ' ', ' ',' ',' ', ' ', ' ',' ',' '};
    Serial.readBytesUntil('\n', receivedPos, 14);
    
    if (receivedPos[0] == '0') {
      turnColorOn(255,0,0); // red
    } else if (receivedPos[0] == '1') {
      turnColorOn(0,255,0); // green
    } else if (receivedPos[0] == '2') {
      turnColorOn(0,0,255); // blue
    } else if (receivedPos[0] == 'A'){
      turnColorOff();
    }
    
//    char *pEnd;
//    int led, ledToAdd, c1, c2;
//
//    led = strtol(receivedPos, &pEnd, 10);
//    ledToAdd = strtol(pEnd, &pEnd, 10);
//    c1 = strtol(pEnd, &pEnd, 10);
//    c2 = strtol(pEnd, NULL, 10);
//
//    turnOnLED(led, 3000, c2, c1, 0, ledToAdd);
  }
}
