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

void initialize() {
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
}