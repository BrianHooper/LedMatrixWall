#include "LedDriver.h"
#include "pride.h"

CRGB leds[NUM_LEDS];
int led_positions[NUM_LEDS];

void setup() {
    initialize(leds, led_positions);
}

void loop() {
    delay(25);
    pride(leds, led_positions);
    FastLED.show();  
}
