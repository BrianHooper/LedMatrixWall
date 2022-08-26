#include <FastLED.h>

#define FASTLED_ALLOW_INTERRUPTS 0
#define NUM_LEDS 300

CRGB leds[NUM_LEDS];

void setup() {
  FastLED.addLeds<WS2812B, 2, GRB>(leds, NUM_LEDS);
  rainbow();
}

void set(CRGB color) {
  for(int i = 0; i < NUM_LEDS; i++) {
    leds[i] = color;
  }
  FastLED.show();
}

void advance() {
    CRGB first = leds[1];
    for(int i = 0; i < NUM_LEDS - 1; i++) {
        leds[i] = leds[i + 1];
    }
    leds[NUM_LEDS - 1] = first;
    FastLED.show();
}

void rainbow() {
  fill_rainbow(leds, NUM_LEDS, 0, 1);
//  for(int i = 0; i < NUM_LEDS; i++) {
//    if (i % 2 == 0) {
//      leds[i] = CRGB::Black;
//    }
//  }
  fadeToBlackBy(leds, NUM_LEDS, 192);
  FastLED.show();
}

void loop() {
//  set(CRGB::Red);
//  delay(500);
//  set(CRGB::Black);
//  delay(500);
  advance();
  delay(15);
}
