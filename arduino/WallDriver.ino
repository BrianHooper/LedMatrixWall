#include <FastLED.h>

#define FASTLED_ALLOW_INTERRUPTS 0

#define NUM_STRIPS 4
#define NUM_LEDS_PER_STRIP 49 * 5
#define NUM_LEDS NUM_LEDS_PER_STRIP * NUM_STRIPS

const int LEDS_PER_NODE_ROW = 7;
const int LEDS_PER_NODE_COL = 7;
const int NODES_PER_PANEL_ROW = 5;
const int NODES_PER_PANEL_COL = 4;

const int LEDS_PER_NODE = LEDS_PER_NODE_ROW * LEDS_PER_NODE_COL;
const int LEDS_PER_PANEL_ROW = NODES_PER_PANEL_ROW * LEDS_PER_NODE_ROW;
const int LEDS_PER_PANEL_COL = NODES_PER_PANEL_COL * LEDS_PER_NODE_COL;

const int LEDS_PER_ROW_OF_NODES = LEDS_PER_NODE * NODES_PER_PANEL_ROW;

CRGB leds[NUM_LEDS];
int led_positions[NUM_LEDS];

void setup() {
  FastLED.addLeds<WS2812B, 2, RGB>(leds, 0 * NUM_LEDS_PER_STRIP, NUM_LEDS_PER_STRIP);
  FastLED.addLeds<WS2812B, 3, RGB>(leds, 1 * NUM_LEDS_PER_STRIP, NUM_LEDS_PER_STRIP);
  FastLED.addLeds<WS2812B, 4, RGB>(leds, 2 * NUM_LEDS_PER_STRIP, NUM_LEDS_PER_STRIP);
  FastLED.addLeds<WS2812B, 5, RGB>(leds, 3 * NUM_LEDS_PER_STRIP, NUM_LEDS_PER_STRIP);
  for(int i = 0; i < NUM_LEDS; i++) {
    int updatedPosition = GridIndexToPanelPositions(i);
    led_positions[i] = GridIndexToPanelPositions(i);
    leds[updatedPosition] = CRGB::Black;
  }
  FastLED.show();
  //Serial.begin(28800);
  //delay(5000);
  //Serial.write(100);
}

int PanelPositionsToPanelIndex(int PanelRowIdx, int PanelColIdx, int NodeRowIdx, int NodeColIdx)
{
    int index = LEDS_PER_ROW_OF_NODES * PanelRowIdx;
    index += PanelColIdx * LEDS_PER_NODE;
    index += NodeColIdx * LEDS_PER_NODE_COL;
    if (NodeColIdx % 2 == 0)
    {
        index += NodeRowIdx;
    } 
    else
    {
        index += LEDS_PER_NODE_COL - (NodeRowIdx + 1);
    }
    return index;
}

int GridIndexToPanelPositions(int index)
{
    int PanelRowIdx = index / LEDS_PER_ROW_OF_NODES;
    int PanelColIdx = (index % LEDS_PER_PANEL_ROW) / LEDS_PER_NODE_ROW;
    int NodeIdx = index - LEDS_PER_ROW_OF_NODES * PanelRowIdx;
    int NodeRowIdx = NodeIdx / LEDS_PER_PANEL_ROW;
    int NodeColIdx = NodeIdx % LEDS_PER_NODE_ROW;

    return PanelPositionsToPanelIndex(PanelRowIdx, PanelColIdx, NodeRowIdx, NodeColIdx);
}

void Clear() {
  for(int i = 0; i < NUM_LEDS; i++) {
    int updatedPosition = led_positions[i];
    leds[updatedPosition] = CRGB::Black;
  }
}

const int BUFFER_SIZE = 16;
byte readBuffer[BUFFER_SIZE];

void processData() {
  int rlen = Serial.readBytes(readBuffer, BUFFER_SIZE);
  if (rlen < 2) {
    return;
  }

  byte parityFlag = readBuffer[0];
  if (parityFlag != 0x69) {
    return;
  }

  byte operationFlag = readBuffer[1];
  if (operationFlag == 0xF8) {
    FastLED.show();   
  }
  else if (operationFlag == 0xB7) {
    Clear();
  }
  else if (operationFlag == 0xA1) {
    int index = (readBuffer[2] << 8) | readBuffer[3];
    CRGB pixelColor = CRGB(readBuffer[4], readBuffer[5], readBuffer[6]);
    int position = led_positions[index];
    leds[position] = pixelColor;
  }    
}

// This function draws rainbows with an ever-changing,
// widely-varying set of parameters.
void pride() 
{
  static uint16_t sPseudotime = 0;
  static uint16_t sLastMillis = 0;
  static uint16_t sHue16 = 0;
 
  uint8_t sat8 = beatsin88( 87, 220, 250);
  uint8_t brightdepth = beatsin88( 341, 96, 224);
  uint16_t brightnessthetainc16 = beatsin88( 203, (25 * 256), (40 * 256));
  uint8_t msmultiplier = beatsin88(147, 23, 60);

  uint16_t hue16 = sHue16;//gHue * 256;
  uint16_t hueinc16 = beatsin88(113, 1, 3000);
  
  uint16_t ms = millis();
  uint16_t deltams = ms - sLastMillis ;
  sLastMillis  = ms;
  sPseudotime += deltams * msmultiplier;
  sHue16 += deltams * beatsin88( 400, 5,9);
  uint16_t brightnesstheta16 = sPseudotime;
  
  for( uint16_t i = 0 ; i < NUM_LEDS; i++) {
    hue16 += hueinc16;
    uint8_t hue8 = hue16 / 256;

    brightnesstheta16  += brightnessthetainc16;
    uint16_t b16 = sin16( brightnesstheta16  ) + 32768;

    uint16_t bri16 = (uint32_t)((uint32_t)b16 * (uint32_t)b16) / 65536;
    uint8_t bri8 = (uint32_t)(((uint32_t)bri16) * brightdepth) / 65536;
    bri8 += (255 - brightdepth);
    
    CRGB newcolor = CHSV( hue8, sat8, bri8);
    
    uint16_t pixelnumber = i;
    pixelnumber = (NUM_LEDS-1) - pixelnumber;
    
    nblend( leds[pixelnumber], newcolor, 64);
  }
}

int XYToIndex(int x, int y) {
  if (x < 0 || y < 0 || x >= 35 || y >= 28) {
    return -1;
  }
  return led_positions[y * 35 + x];
}

void Set(int index, CRGB color) {
  if (index < 0 || index >= 980) {
    return;
  }
  leds[index] = color;
}

void DrawH(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x, y + 2), color);
  Set(XYToIndex(x, y + 3), color);
  Set(XYToIndex(x, y + 4), color);

  Set(XYToIndex(x + 1, y + 2), color);

  Set(XYToIndex(x + 2, y), color);
  Set(XYToIndex(x + 2, y + 1), color);
  Set(XYToIndex(x + 2, y + 2), color);
  Set(XYToIndex(x + 2, y + 3), color);
  Set(XYToIndex(x + 2, y + 4), color);
}

void DrawA(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x, y + 2), color);
  Set(XYToIndex(x, y + 3), color);
  Set(XYToIndex(x, y + 4), color);

  Set(XYToIndex(x + 1, y), color);
  Set(XYToIndex(x + 1, y + 2), color);

  Set(XYToIndex(x + 2, y), color);
  Set(XYToIndex(x + 2, y + 1), color);
  Set(XYToIndex(x + 2, y + 2), color);
  Set(XYToIndex(x + 2, y + 3), color);
  Set(XYToIndex(x + 2, y + 4), color);
}

void DrawP(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x + 1, y), color);
  Set(XYToIndex(x + 2, y), color);
  
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x + 2, y + 1), color);

  Set(XYToIndex(x, y + 2), color);
  Set(XYToIndex(x + 1, y + 2), color);
  Set(XYToIndex(x + 2, y + 2), color);

  Set(XYToIndex(x, y + 3), color);
  Set(XYToIndex(x, y + 4), color);
}

void DrawY(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x, y + 2), color);

  Set(XYToIndex(x + 1, y + 2), color);
  Set(XYToIndex(x + 1, y + 3), color);
  Set(XYToIndex(x + 1, y + 4), color);

  Set(XYToIndex(x + 2, y), color);
  Set(XYToIndex(x + 2, y + 1), color);
  Set(XYToIndex(x + 2, y + 2), color);
}

void DrawT(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x + 1, y), color);
  Set(XYToIndex(x + 2, y), color);

  Set(XYToIndex(x + 1, y + 1), color);
  Set(XYToIndex(x + 1, y + 2), color);
  Set(XYToIndex(x + 1, y + 3), color);
  Set(XYToIndex(x + 1, y + 4), color);
}

void DrawI(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x, y + 2), color);
  Set(XYToIndex(x, y + 3), color);
  Set(XYToIndex(x, y + 4), color);
}

void DrawR(int x, int y, CRGB color) {
  Set(XYToIndex(x, y), color);
  Set(XYToIndex(x, y + 1), color);
  Set(XYToIndex(x, y + 2), color);
  Set(XYToIndex(x, y + 3), color);
  Set(XYToIndex(x, y + 4), color);
  
  Set(XYToIndex(x + 1, y), color);
  Set(XYToIndex(x + 2, y), color);
  Set(XYToIndex(x + 3, y), color);

  Set(XYToIndex(x + 3, y + 1), color);

  Set(XYToIndex(x + 1, y + 2), color);
  Set(XYToIndex(x + 2, y + 2), color);
  Set(XYToIndex(x + 3, y + 2), color);

  Set(XYToIndex(x + 2, y + 3), color);
  Set(XYToIndex(x + 3, y + 4), color);
}

void DrawThirth(int offset) {
  DrawH(4 + offset, 4, CRGB::Red);
  DrawA(7 + offset, 4, CRGB::White);
  DrawP(11 + offset, 4, CRGB::Blue);
  DrawP(15 + offset, 4, CRGB::Red);
  DrawY(19 + offset, 4, CRGB::White);
  
  DrawT(7 + offset, 13, CRGB::Blue);
  DrawH(11 + offset, 13, CRGB::Red);
  DrawI(15 + offset, 13, CRGB::White);
  DrawR(17 + offset, 13, CRGB::Blue);
  DrawT(22 + offset, 13, CRGB::Red);
  DrawH(26 + offset, 13, CRGB::White);
}

void HappyThirth() {
  delay(500);
  for(int i = -35; i < 35; i++) {
    Clear();
    DrawThirth(i);
    FastLED.show();
    if (i == 0) {
      delay(2000);
    }
    else {
      delay(100);
    }
  }
  delay(500);
}

void loop() {
  delay(25);
  pride();
  FastLED.show();  
}
