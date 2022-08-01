#include "LedDriver.h"

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