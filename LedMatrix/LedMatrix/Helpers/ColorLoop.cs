using System.Drawing;

namespace LedMatrix.Helpers
{
    public class ColorLoop
    {
        private int R;
        private int G;
        private int B;

        private enum State
        {
            rIncreasing,  
            gIncreasing, 
            bIncreasing,
            rDecreasing, 
            gDecreasing,
            bDecreasing
        }

        private State state;

        public ColorLoop()
        {
            this.R = 219;
            this.G = 42;
            this.B = 42;
            this.state = State.gIncreasing;
        }

        private Color ToColor()
        {
            return Color.FromArgb(255, this.R, this.G, this.B);
        }

        public Color Next()
        {
            var current = this.ToColor();

            if (this.state == State.rIncreasing)
            {
                if (this.R >= 219)
                {
                    this.B--;
                    this.state = State.bDecreasing;
                }
                else
                {
                    this.R++;
                }
            }
            else if (this.state == State.gIncreasing) 
            { 
                if (this.G >= 219)
                {
                    this.R--;
                    this.state = State.rDecreasing;
                }
                else
                {
                    this.G++;
                }
            }
            else if (this.state == State.bIncreasing) 
            {
                if (this.B >= 219)
                {
                    this.G--;
                    this.state = State.gDecreasing;
                }
                else
                {
                    this.B++;
                }
            }
            else if (this.state == State.rDecreasing) 
            {
                if (this.R <= 42)
                {
                    this.B++;
                    this.state = State.bIncreasing;
                }
                else
                {
                    this.R--;
                }
            }
            else if (this.state == State.gDecreasing) 
            {
                if (this.G <= 42)
                {
                    this.R++;
                    this.state = State.rIncreasing;
                }
                else
                {
                    this.G--;
                }
            }
            else //if (this.state == State.bDecreasing) 
            {
                if (this.B <= 42)
                {
                    this.G++;
                    this.state = State.gIncreasing;
                }
                else
                {
                    this.B--;
                }
            }

            return current;
        }

        public List<Color> NextFrame()
        {
            var frame = new List<Color>();
            for (int j = 0; j < Constants.TotalLeds; j++)
            {
                frame.Add(this.Next());
            }
            return frame;
        }
    }
}
