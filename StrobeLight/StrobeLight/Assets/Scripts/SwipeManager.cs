using UnityEngine;

public class SwipeManager : MonoBehaviour
{
	public enum Swipe { Up, Down, Left, Right, None, UpLeft, UpRight, DownLeft, DownRight, SwipingDown, SwipingUp };
	public float minSwipeLength = 200f;
	public GUIText debugInfo;
	
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	public Vector2 CurrentSwipe;
	
	float tweakFactor = 0.5f;
	
	public static Swipe swipeDirection;
	
	
	void Update ()
	{
		DetectSwipe();
	}
	
	public void DetectSwipe ()
	{
		if (Input.touches.Length > 0) {
			Touch t = Input.GetTouch(0);

			secondPressPos = new Vector2(t.position.x, t.position.y);
			CurrentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

			// Make sure it was a legit swipe, not a tap
			if (CurrentSwipe.magnitude < minSwipeLength) {
				debugInfo.text = "Tapped";
				swipeDirection = Swipe.None;
				return;
			}
			
			CurrentSwipe.Normalize();
			
			debugInfo.text = CurrentSwipe.x.ToString() + " " + CurrentSwipe.y.ToString();
			
			// Swipe up
			if (CurrentSwipe.y > 0 && CurrentSwipe.x > 0 - tweakFactor && CurrentSwipe.x < tweakFactor) {
				swipeDirection = Swipe.SwipingUp;
				debugInfo.text = "Up swiping";
				
				// Swipe down
			} else if (CurrentSwipe.y < 0 && CurrentSwipe.x > 0 - tweakFactor && CurrentSwipe.x < tweakFactor) {
				swipeDirection = Swipe.SwipingDown;
				debugInfo.text = "Down swiping";
			}

			if (t.phase == TouchPhase.Began) {
				firstPressPos = new Vector2(t.position.x, t.position.y);
			}
			
			if (t.phase == TouchPhase.Ended) {
				secondPressPos = new Vector2(t.position.x, t.position.y);
				CurrentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				// Make sure it was a legit swipe, not a tap
				if (CurrentSwipe.magnitude < minSwipeLength) {
					debugInfo.text = "Tapped";
					swipeDirection = Swipe.None;
					return;
				}
				
				CurrentSwipe.Normalize();
				
				debugInfo.text = CurrentSwipe.x.ToString() + " " + CurrentSwipe.y.ToString();
				
				// Swipe up
				if (CurrentSwipe.y > 0 && CurrentSwipe.x > 0 - tweakFactor && CurrentSwipe.x < tweakFactor) {
					swipeDirection = Swipe.Up;
					debugInfo.text = "Up swipe";
					
					// Swipe down
				} else if (CurrentSwipe.y < 0 && CurrentSwipe.x > 0 - tweakFactor && CurrentSwipe.x < tweakFactor) {
					swipeDirection = Swipe.Down;
					debugInfo.text = "Down swipe";
					
					// Swipe left
				} else if (CurrentSwipe.x < 0 && CurrentSwipe.y > 0 - tweakFactor && CurrentSwipe.y < tweakFactor) {
					swipeDirection = Swipe.Left;
					debugInfo.text = "Left swipe";
					
					// Swipe right
				} else if (CurrentSwipe.x > 0 && CurrentSwipe.y > 0 - tweakFactor && CurrentSwipe.y < tweakFactor) {
					swipeDirection = Swipe.Right;
					debugInfo.text = "Right swipe";
					
					// Swipe up left
				} else if (CurrentSwipe.y > 0 && CurrentSwipe.x < 0 ) {
					swipeDirection = Swipe.UpLeft;
					debugInfo.text = "Up Left swipe";
					
					// Swipe up right
				} else if (CurrentSwipe.y > 0 && CurrentSwipe.x > 0 ) {
					swipeDirection = Swipe.UpRight;
					debugInfo.text = "Up Right swipe";
					
					// Swipe down left
				} else if (CurrentSwipe.y < 0 && CurrentSwipe.x < 0 ) {
					swipeDirection = Swipe.DownLeft;
					debugInfo.text = "Down Left swipe";
					
					// Swipe down right
				} else if (CurrentSwipe.y < 0 && CurrentSwipe.x > 0 ) {
					swipeDirection = Swipe.DownRight;
					debugInfo.text = "Down Right swipe";
				}
			}
		} else {
			swipeDirection = Swipe.None; 
			//debugInfo.text = "No swipe"; // if you display this, you will lose the debug text when you stop swiping
		}


	}
}