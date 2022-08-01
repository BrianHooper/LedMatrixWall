#include <FastLED.h>

#define FASTLED_ALLOW_INTERRUPTS 0

#define NUM_STRIPS 4
#define NUM_LEDS_PER_STRIP 49 * 5
#define NUM_LEDS NUM_LEDS_PER_STRIP * NUM_STRIPS

#define LEDS_PER_NODE_ROW 7
#define LEDS_PER_NODE_COL 7
#define NODES_PER_PANEL_ROW 5
#define NODES_PER_PANEL_COL 4

int PanelPositionsToPanelIndex(int PanelRowIdx, int PanelColIdx, int NodeRowIdx, int NodeColIdx)
{
    int LEDS_PER_NODE = 7 * LEDS_PER_NODE_COL;
    int LEDS_PER_ROW_OF_NODES = LEDS_PER_NODE * NODES_PER_PANEL_ROW;

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
    int LEDS_PER_NODE = 7 * LEDS_PER_NODE_COL;
    int LEDS_PER_ROW_OF_NODES = LEDS_PER_NODE * NODES_PER_PANEL_ROW;
    int LEDS_PER_PANEL_ROW = NODES_PER_PANEL_ROW * 7;
    int LEDS_PER_PANEL_COL = NODES_PER_PANEL_COL * LEDS_PER_NODE_COL;

    int PanelRowIdx = index / LEDS_PER_ROW_OF_NODES;
    int PanelColIdx = (index % LEDS_PER_PANEL_ROW) / 7;
    int NodeIdx = index - LEDS_PER_ROW_OF_NODES * PanelRowIdx;
    int NodeRowIdx = NodeIdx / LEDS_PER_PANEL_ROW;
    int NodeColIdx = NodeIdx % 7;

    return PanelPositionsToPanelIndex(PanelRowIdx, PanelColIdx, NodeRowIdx, NodeColIdx);
}

void Clear(CRGB* leds, int* led_positions) {
  for(int i = 0; i < NUM_LEDS; i++) {
    int updatedPosition = led_positions[i];
    leds[updatedPosition] = CRGB::Black;
  }
}

int XYToIndex(int x, int y, int* led_positions) {
    if (x < 0 || y < 0 || x >= 35 || y >= 28) {
        return -1;
    }
    return led_positions[y * 35 + x];
}

void Set(int index, CRGB color, CRGB* leds) {
    if (index < 0 || index >= 980) {
        return;
    }
    leds[index] = color;
}

void initialize(CRGB* leds, int* led_positions) {
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
