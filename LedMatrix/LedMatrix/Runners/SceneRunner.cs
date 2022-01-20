using LedMatrix.Helpers;
using LedMatrix.Models;

namespace LedMatrix.Runners
{
    public class SceneRunner : RunnerBase
    {
        /*  A Scene consists of a folder of serialized frame objects that are deserialzied and shown as still images
         *  Each frame should have a "pause" that allows it to be shown for a specific number of cycles
         *  
         *  A movie is a list of scenes shown on a loop, which may be separated by transitions
         *  Each movie is driven by a config file, which contains a list of scene folders
         *  As well as some metadata, like whether to show the scenes sequentially or randomized
         *      [MovieName]
         *      randomize=false
         *      
         *      [Scenes]
         *      Scene=/scenes/movie1/scene1
         *      Transition=ScrollFromLeft:Speed=3;UsePreviousFrame=true;UseNextFrame=false
         *  
         *  Transitions (like scrolling an image left to right) get calculated at runtime
         *  They should take a "previousframe" and "nextframe" parameter, and create a new scene with a transition between the two
         *  They should not modify the state of the input frames, just return a list of frames with the calculated
         *  The SceneRunner should automatically handle inserting a blank frame as the previous or next frame if there isn't another scene to use
         *  
         *  We will have one master SceneRunner config, which handles choosing which movie to display - likely just based on the date
         * 
         *  We will need a blocking fixed-size queue to load each frame into memory, without loading the whole scene at once
         *  No need to set some timer for reading the queue, as the fps is handled by the controller
         *  
         *  We will need one constant "blank" frame, easiest to just keep that in memory so it can be re-used
         *  
         *  Handling the frame pause
         *      The led controller uses a queue to send the frames to the leds at a steady pace
         
         *          For the GameRunner and CameraRunner, we want to keep the size of the controller's frame queue small
         *          Better to drop some frames at the controller stage, than to have too much of a delay between the game data and what the led's are displaying
         *          
         *          For the SceneRunner, we want the opposite, we allow the controller's frame queue to grow much larger (still need a limit for memory reasons)
         *          Instead of having separate "paint with limit" method in the controller, just pass the queue max size from the runner
         *          
         *          Each runner should define its own queue limit, when changing runners, we clear the queue, and reset the max frame queue limit 
         *          to the value from the new runner
         *          
         *          We should set a timer in the SceneRunner to send frames at the same fps as the controller. Since whenever the controller's queue is empty,
         *          it just waits for new frames and doesn't re-paint the panel, we can handle the frame pause in the Runner
         *          We can just "peek" at the top of the queue, decrement the value of the "pause", and deque/send to controller if it is == 0
         * 
         */
        public SceneRunner(ControllerBase ledController) : base(RunnerType.SandboxRunner, ledController, Constants.DeferredQueueLimit)
        {
            
        }

        protected override void Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
