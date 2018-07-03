using UnityEngine ;
using System.Collections ;
using UnityEngine.UI ;

public class GUIEffects : MonoBehaviour {

	// common params
	[HideInInspector] public 		float 			commonSpeed 						= 1.0f ;
	[HideInInspector] public 		float 			commonStartDelay 				= 0f ;
	[HideInInspector] public 		float 			commonDismissDelay 			= 0f ;
	[HideInInspector] public GUITween.EaseType commonEaseType 		= GUITween.EaseType.linear;
	[HideInInspector] public GUITween.LoopType commonLoopType 		= GUITween.LoopType.none;

	// fade variable
	[HideInInspector] public 		float 			fadeSpeed 							= 1.0f ;
	[HideInInspector] public 		float 			fadeDelay 							= 0f ;
	[HideInInspector] public 		float 			fadeDismissDelay 				= 0f ;
	[HideInInspector] public 		float 			fadeStartValue 						= 0f ;
	[HideInInspector] public 		float 			fadeEndValue 						= 1.0f ;
	[HideInInspector] public GUITween.LoopType fadeLoopType 			= GUITween.LoopType.none;

	// move from variables
	[HideInInspector] public 		float 			positionEffectSpeed 				= 1.0f ;
	[HideInInspector] public 		float 			positionEffectDelay 				= 0f ;
	[HideInInspector] public 		float 			positionEffectDismissDelay 	= 0f ;
	[HideInInspector] public 		bool 			isRandomPositionEaseType 	= false ;
	[HideInInspector] public GUITween.EaseType positionEaseType 		= GUITween.EaseType.linear;
	[HideInInspector] public GUITween.LoopType positionLoopType 		= GUITween.LoopType.none;

	// scale variables
	[HideInInspector] public 		float 			scaleEffectSpeed 					= 1.0f ;
	[HideInInspector] public 		float 			scaleEffectDelay		 			= 0f ;
	[HideInInspector] public 		float 			scaleEffectDismissDelay 		= 0f ;
	[HideInInspector] public 		float 			popUpStartValue 					= 0f ;
	[HideInInspector] public 		float 			reversePopUpStartValue 		= 5.0f ;
	[HideInInspector] public 		bool 			isRandomScaleEaseType 		= false ;
	[HideInInspector] public GUITween.EaseType scaleEaseType 				= GUITween.EaseType.linear;
	[HideInInspector] public GUITween.LoopType scaleLoopType 			= GUITween.LoopType.none;

	// rotation variables
	[HideInInspector] public 		float 			rotationSpeed 						= 1.0f ;
	[HideInInspector] public 		float 			rotationDelay 						= 0f ;
	[HideInInspector] public 		float 			rotationDismissDelay 			= 0f ;
	[HideInInspector] public 		float 			rotationDismissTime 			= 1.0f ;
	[HideInInspector] public 		bool 			isContinuosRotation 			= false ;
	[HideInInspector] public  		bool 			axisX 									= false ;
	[HideInInspector] public 		bool 			axisY 									= false ;
	[HideInInspector] public 		bool 			axisZ 									= false ;
	[HideInInspector] public 		int 			numberOfRotations 				= 1 ;
	[HideInInspector] public 		int 			flipsPerRotation 					= 1 ;
	[HideInInspector] public 		float 			delayBetweenTwoRotation 	= 0.01f;
	[HideInInspector] public 		bool 			isRandomRotationEaseType 	= false ;
	[HideInInspector] public GUITween.EaseType rotationEaseType 		= GUITween.EaseType.linear;
	[HideInInspector] public GUITween.LoopType rotationLoopType 		= GUITween.LoopType.none;

	// all booleans
	[HideInInspector] public	 	bool 			ignoreTimeScale 					= false ;
	[HideInInspector] public 		bool 			isClickable 							= false ;
	[HideInInspector] public 		bool 			isDestroyOnDismiss 				= false ;
	[HideInInspector] public 		bool 			isCommonParams 				= false ;
	[HideInInspector] public 		bool 			isPanel 									= false ;
	[HideInInspector] public 		bool 			isRandom 							= false ;
	[HideInInspector] public 		bool 			isRandomEaseType 				= true ;
	[HideInInspector] public 		bool 			isColorChangeFoldOpen 		= false ;
	[HideInInspector] public 		bool 			isFade 									= false ;
	[HideInInspector] public 		bool 			isPositionChangeFoldOpen 	= false ;
	[HideInInspector] public 		bool 			isMoveFromBottom 				= false ;
	[HideInInspector] public 		bool 			isMoveFromLeft 					= false ;
	[HideInInspector] public 		bool 			isMoveFromRight 					= false ;
	[HideInInspector] public 		bool 			isMoveFromTop 					= false ;
	[HideInInspector] public 		bool 			isMoveFromTopRight 			= false ;
	[HideInInspector] public 		bool 			isMoveFromTopLeft 				= false ;
	[HideInInspector] public 		bool 			isMoveFromBottomRight 		= false ;
	[HideInInspector] public 		bool 			isMoveFromBottomLeft 		= false ;
	[HideInInspector] public 		bool 			isScaleChangeFoldOpen 		= false ;
	[HideInInspector] public 		bool 			isPopUp 								= false ;
	[HideInInspector] public 		bool 			isReversePopUp 					= false ;
	[HideInInspector] public 		bool 			isRotationChangeFoldOpen 	= false ;
	[HideInInspector] public 		bool 			isRotation 							= false ;

	// dismiss objects on click
	public 						GameObject[] 		dismissOnClick ;

	// booleans
	private 		bool 			isButton 								= false ;
	private 		bool 			isText 									= false ;
	private 		bool 			isImage 								= false ;
	private 		bool 			isRawImage 							= false ;
	private 		bool 			isSlider 								= false ;
	private 		bool 			isScrollBar 							= false ;
	private 		bool 			isToggle 								= false ;
	private 		bool 			isInputField 							= false ;


	private 		float 			tempFadeAlpha ;
	private 		float 			tempFadeAlphaAddition ;
	private 		float 			tempFadeFrames 					= 0f ;
	private 		bool 			fadeCompleted 					= false ;
	private 		int 			tempNumberOfRotations 		= 0 ;
	private 		Vector3 	rotationVector ;
	private 		float 			inFieldPlaceAlphaDevider 		= 1.0f;

	// the variable below is only used in ping pong loop of fade
	private 		bool 			isAlphaIncreasing 				= true;

	// Use this for initialization
	public void Start ( ) {
		tempFadeAlpha 						= fadeStartValue ;
		tempFadeAlphaAddition 			= ( ( fadeEndValue - fadeStartValue ) / ( (2.0f/fadeSpeed) * 50.0f ) ) ;

		WhichObjectIsThis();
		// check if common parameters
		if( isCommonParams ){
			fadeSpeed 					= positionEffectSpeed 				= scaleEffectSpeed				= rotationSpeed 				= commonSpeed ;
			fadeDelay 					= positionEffectDelay 				= scaleEffectDelay				= rotationDelay 				= commonStartDelay ;
			fadeDismissDelay 		= positionEffectDismissDelay 	= scaleEffectDismissDelay 	= rotationDismissDelay 	= commonDismissDelay ;
													positionEaseType 					= scaleEaseType 					= rotationEaseType 			= commonEaseType;
			fadeLoopType 				= positionLoopType 					= scaleLoopType 					= rotationLoopType 			= commonLoopType;
		}

		// check if random
		if( isRandom ){
			ApplyRandomEffects();
		}

		// get placeholder's alpha of input field.
		if( isInputField ){
			if( transform.childCount > 1 ){
				Transform placeHold = transform.GetChild( transform.childCount - 2 );
				inFieldPlaceAlphaDevider = 1.0f/(placeHold.GetComponent<Text>().color.a );
			}
		}

		// checking for random easetype of all objects
		if( isRandomPositionEaseType ){
			GetRandomEaseType("Position");
		}
		if(isRandomRotationEaseType){
			GetRandomEaseType("Rotation");
		}
		if(isRandomScaleEaseType){
			GetRandomEaseType("Scale");
		}

		if( isMoveFromBottom ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromLeft ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromRight ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromTop ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromTopRight ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromTopLeft ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromBottomRight ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isMoveFromBottomLeft ){
			GUITween.MoveFrom( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", positionLoopType ));
		}
		if( isPopUp ){
			GUITween.ScaleFrom( gameObject , GUITween.Hash("scale", new Vector3( popUpStartValue, popUpStartValue, popUpStartValue ), "islocal", true, "time", 2.0f/scaleEffectSpeed, "delay", scaleEffectDelay, "easeType", scaleEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", scaleLoopType ));
		}
		if( isReversePopUp ){
			GUITween.ScaleFrom( gameObject , GUITween.Hash("scale", new Vector3( reversePopUpStartValue, reversePopUpStartValue, reversePopUpStartValue ), "islocal", true, "time", 2.0f/scaleEffectSpeed, "delay", scaleEffectDelay, "easeType", scaleEaseType, "ignoretimescale" , ignoreTimeScale, "looptype", scaleLoopType ));
		}
		if( isRotation ){
			if( axisX ){
				rotationVector = new Vector3( flipsPerRotation, 0, 0 );
			}else
			if( axisY ){
				rotationVector = new Vector3( 0, flipsPerRotation, 0);
			}else
			if( axisZ ){
				rotationVector = new Vector3( 0, 0, flipsPerRotation );
			}else{
				rotationVector = new Vector3( flipsPerRotation, flipsPerRotation, flipsPerRotation );
				Debug.LogError("You have not selected your axis to rotate in GUIEffects. Please select any one axis in the inspector on your GUI element.");
			}
			if( rotationLoopType == GUITween.LoopType.none ){
				if( isContinuosRotation ){
					GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", rotationDelay, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateContinuos" ));
				}else{
					GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", rotationDelay, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateNonContinuos" ));
				}
			}else{
				GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", rotationDelay, "looptype", rotationLoopType, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale ));
			}

		}

	}
	// continue rotations
	void WaitAndRotateContinuos(){
		GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", delayBetweenTwoRotation, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateContinuos" ));
	}
	// limited rotations
	void WaitAndRotateNonContinuos(){
		tempNumberOfRotations++;
		if( tempNumberOfRotations < (numberOfRotations) ){
			GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", delayBetweenTwoRotation, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateNonContinuos" ));
			Debug.Log(tempNumberOfRotations);
		}
	}



	// fixed update
	void FixedUpdate(){
		tempFadeFrames++;
		// fading effect
		if( isFade && tempFadeFrames >= fadeDelay * 50 ){
			if( isButton ){
				Color c = gameObject.GetComponent<Image>().color;
				gameObject.GetComponent<Image>().color = new Color( c.r, c.g, c.b, tempFadeAlpha );
				if( transform.childCount > 0 ){
					Transform childText = transform.GetChild( 0 );
					Color cc = childText.gameObject.GetComponent<Text>().color;
					childText.gameObject.GetComponent<Text>().color = new Color( cc.r, cc.g, cc.b, tempFadeAlpha );
				}
				ChangeAlpha();
			}
			if( isText ){
				gameObject.GetComponent<Text>().color = new Color( gameObject.GetComponent<Text>().color.r, gameObject.GetComponent<Text>().color.g, gameObject.GetComponent<Text>().color.b, tempFadeAlpha );
				ChangeAlpha();
			}
			if( isImage ){
				gameObject.GetComponent<Image>().color = new Color( gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, tempFadeAlpha );
				ChangeAlpha();
			}
			if( isRawImage ){
				gameObject.GetComponent<RawImage>().color = new Color( gameObject.GetComponent<RawImage>().color.r, gameObject.GetComponent<RawImage>().color.g, gameObject.GetComponent<RawImage>().color.b, tempFadeAlpha );
				ChangeAlpha();
			}
			if( isSlider ){
				if( transform.childCount > 0 ){
					Transform bg = transform.GetChild( 0 );
					Color bgc = bg.GetComponent<Image>().color;
					bg.GetComponent<Image>().color = new Color( bgc.r, bgc.g, bgc.b, tempFadeAlpha );
					Transform fill = transform.GetChild( 1 ).transform.GetChild( 0 );
					Color fc = fill.GetComponent<Image>().color;
					fill.GetComponent<Image>().color = new Color( fc.r, fc.g, fc.b, tempFadeAlpha );
					Transform handle = transform.GetChild( 2 ).transform.GetChild( 0 );
					Color hc = handle.GetComponent<Image>().color;
					handle.GetComponent<Image>().color = new Color( hc.r, hc.g, hc.b, tempFadeAlpha );
				}
				ChangeAlpha();
			}
			if( isScrollBar ){
				gameObject.GetComponent<Image>().color = new Color( gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, tempFadeAlpha );
				if( transform.childCount >0 ){
					Transform hand = transform.GetChild(0).transform.GetChild(0);
					Color hdc = hand.GetComponent<Image>().color;
					hand.GetComponent<Image>().color = new Color( hdc.r, hdc.g, hdc.b, tempFadeAlpha );
				}
				ChangeAlpha();
			}
			if( isToggle ){
				if( transform.childCount > 0 ){
					Transform bgToggle = transform.GetChild(0);
					Color bgtc = bgToggle.GetComponent<Image>().color;
					bgToggle.GetComponent<Image>().color = new Color( bgtc.r, bgtc.g, bgtc.b, tempFadeAlpha );
					Transform checkmark = transform.GetChild(0).transform.GetChild(0);
					Color chc = checkmark.GetComponent<Image>().color;
					checkmark.GetComponent<Image>().color = new Color( chc.r, chc.g, chc.b, tempFadeAlpha );
					Transform label = transform.GetChild(1);
					Color lc = label.GetComponent<Text>().color;
					label.GetComponent<Text>().color = new Color( lc.r, lc.g, lc.b, tempFadeAlpha );
				}
				ChangeAlpha();
			}
			if( isInputField ){
				gameObject.GetComponent<Image>().color = new Color( gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, tempFadeAlpha );
				if( transform.childCount > 1 ){
					Transform placeHold = transform.GetChild( transform.childCount - 2 );
					placeHold.GetComponent<Text>().color = new Color( placeHold.GetComponent<Text>().color.r, placeHold.GetComponent<Text>().color.g, placeHold.GetComponent<Text>().color.b , tempFadeAlpha/inFieldPlaceAlphaDevider );
					Transform inputText = transform.GetChild( transform.childCount - 1 );
					inputText.GetComponent<Text>().color = new Color( inputText.GetComponent<Text>().color.r, inputText.GetComponent<Text>().color.g, inputText.GetComponent<Text>().color.b , tempFadeAlpha );
				}
				ChangeAlpha();
			}
		}

	}

	// change alpha
	void ChangeAlpha(){
		switch( fadeLoopType ){
		case GUITween.LoopType.none:
			if( !fadeCompleted && tempFadeAlpha <= fadeEndValue ){
				tempFadeAlpha += tempFadeAlphaAddition ;
			}
			if( fadeCompleted && tempFadeAlpha >= fadeStartValue ){
				tempFadeAlpha -= tempFadeAlphaAddition ;
			}
			break;

		case GUITween.LoopType.loop:
			if( tempFadeAlpha >= fadeEndValue ){
				tempFadeAlpha = fadeStartValue;
			}
			if( tempFadeAlpha <= fadeEndValue ){
				tempFadeAlpha += tempFadeAlphaAddition ;
			}
			break;

		case GUITween.LoopType.pingPong:
			if( tempFadeAlpha <= fadeStartValue ){
				isAlphaIncreasing = true;
			}
			if( tempFadeAlpha >= fadeEndValue ){
				isAlphaIncreasing = false;
			}
			if( isAlphaIncreasing ){
				tempFadeAlpha += tempFadeAlphaAddition;
			}else{
				tempFadeAlpha -= tempFadeAlphaAddition;
			}
			break;
		}
	}

	// check which object is this
	void WhichObjectIsThis(){
		if( gameObject.GetComponent<Button>() == true && gameObject.GetComponent<Image>() == true ){
			isButton = true;
		}
		if( gameObject.GetComponent<Text>() == true ){
			isText = true;
		}
		if( gameObject.GetComponent<Image>() == true && gameObject.GetComponent<Button>() == false && gameObject.GetComponent<Scrollbar>() == false && gameObject.GetComponent<InputField>() == false && isPanel == false ){
			isImage = true;
		}
		if( gameObject.GetComponent<RawImage>() == true ){
			isRawImage = true;
		}
		if( gameObject.GetComponent<Slider>() == true ){
			isSlider = true;
		}
		if( gameObject.GetComponent<Scrollbar>() == true && gameObject.GetComponent<Image>() == true ){
			isScrollBar = true;
		}
		if( gameObject.GetComponent<Toggle>() == true ){
			isToggle = true;
		}
		if( gameObject.GetComponent<InputField>() == true && gameObject.GetComponent<Image>() == true ){
			isInputField = true;
		}
		return;
	}

	// random
	public void ApplyRandomEffects(){
		isFade = isMoveFromBottom = isMoveFromLeft = isMoveFromRight = isMoveFromTop = isMoveFromBottomLeft = isMoveFromBottomRight = isMoveFromTopLeft = isMoveFromTopRight = isPopUp = isReversePopUp = isRotation = false ;
		int totalRandomEffects = Random.Range( 1, 5 );
		switch( totalRandomEffects ){
		case 1:
			int oneRandom = Random.Range( 1,5 );
			ApplyMe( oneRandom );
			break;

		case 2:
												int twoRandomFirst = Random.Range( 1,5 );
			TWO_RANDOM_TRY : 	int twoRandomSecond = Random.Range( 1,5 );
			if( twoRandomSecond == twoRandomFirst ){
				goto TWO_RANDOM_TRY;
			}
			ApplyMe( twoRandomFirst );
			ApplyMe( twoRandomSecond );
			break;

		case 3:
															int threeRandomFirst = Random.Range( 1, 5);
			THREE_RANDOM_TRY_ONE : 	int threeRandomSecond = Random.Range( 1, 5);
			if( threeRandomSecond == threeRandomFirst ){
				goto THREE_RANDOM_TRY_ONE ;
			}
			THREE_RANDOM_TRY_TWO : 	int threeRandomThird = Random.Range( 1, 5);
			if( threeRandomThird == threeRandomFirst || threeRandomThird == threeRandomSecond ){
				goto THREE_RANDOM_TRY_TWO ;
			}
			ApplyMe( threeRandomFirst );
			ApplyMe( threeRandomSecond );
			ApplyMe( threeRandomThird );
			break;

		case 4:
			ApplyMe( 1 );
			ApplyMe( 2 );
			ApplyMe( 3 );
			ApplyMe( 4 );
			break;
		}
	}
	void ApplyMe( int temp ){
		switch( temp ){
		case 1:
			if( isRandomEaseType ){
				GetRandomEaseType("Fade");
			}
			ApplyFadeForRandom();
			break;
		case 2:
			if( isRandomEaseType ){
				GetRandomEaseType("Position");
			}
			ApplyMoveForRandom();
			break;
		case 3:
			if( isRandomEaseType ){
				GetRandomEaseType("Scale");
			}
			ApplyScaleForRandom();
			break;
		case 4:
			if( isRandomEaseType ){
				GetRandomEaseType("Rotation");
			}
			ApplyRotationForRandom();
			break;
		}
	}
	void GetRandomEaseType( string effectName ){
			int whichEaseType = Random.Range(1,33);
			GUITween.EaseType tempEase = GUITween.EaseType.linear;

			switch( whichEaseType ){
			case 1:
				tempEase = GUITween.EaseType.easeInQuad;
				break;
			case 2:
				tempEase = GUITween.EaseType.easeOutQuad;
				break;
			case 3:
				tempEase = GUITween.EaseType.easeInOutQuad;
				break;
			case 4:
				tempEase = GUITween.EaseType.easeInCubic;
				break;
			case 5:
				tempEase = GUITween.EaseType.easeOutCubic;
				break;
			case 6:
				tempEase = GUITween.EaseType.easeInOutCubic;
				break;
			case 7:
				tempEase = GUITween.EaseType.easeOutElastic;
				break;
			case 8:
				tempEase = GUITween.EaseType.easeInQuart;
				break;
			case 9:
				tempEase = GUITween.EaseType.easeOutQuart;
				break;
			case 10:
				tempEase = GUITween.EaseType.easeInOutQuart;
				break;
			case 11:
				tempEase = GUITween.EaseType.easeInQuint;
				break;
			case 12:
				tempEase = GUITween.EaseType.easeOutQuint;
				break;
			case 13:
				tempEase = GUITween.EaseType.easeInOutQuint;
				break;
			case 14:
				tempEase = GUITween.EaseType.easeInSine;
				break;
			case 15:
				tempEase = GUITween.EaseType.easeOutSine;
				break;
			case 16:
				tempEase = GUITween.EaseType.easeInOutElastic;
				break;
			case 17:
				tempEase = GUITween.EaseType.easeInOutSine;
				break;
			case 18:
				tempEase = GUITween.EaseType.easeInExpo;
				break;
			case 19:
				tempEase = GUITween.EaseType.easeOutExpo;
				break;
			case 20:
				tempEase = GUITween.EaseType.easeInOutExpo;
				break;
			case 21:
				tempEase = GUITween.EaseType.easeInCirc;
				break;
			case 22:
				tempEase = GUITween.EaseType.easeOutCirc;
				break;
			case 23:
				tempEase = GUITween.EaseType.easeInOutCirc;
				break;
			case 24:
				tempEase = GUITween.EaseType.linear;
				break;
			case 25:
				tempEase = GUITween.EaseType.spring;
				break;
			case 26:
				tempEase = GUITween.EaseType.easeInBounce;
				break;
			case 27:
				tempEase = GUITween.EaseType.easeOutBounce;
				break;
			case 28:
				tempEase = GUITween.EaseType.easeInOutBounce;
				break;
			case 29:
				tempEase = GUITween.EaseType.easeInBack;
				break;
			case 30:
				tempEase = GUITween.EaseType.easeOutBack;
				break;
			case 31:
				tempEase = GUITween.EaseType.easeInOutBack;
				break;
			case 32:
				tempEase = GUITween.EaseType.easeInElastic;
				break;
			default:
				tempEase = GUITween.EaseType.linear;
				break;
			}

			switch( effectName ){
			case "Fade":
				// nothing
				break;
			case "Position":
				positionEaseType = tempEase;
				break;
			case "Scale":
				scaleEaseType = tempEase;
				break;
			case "Rotation":
				rotationEaseType = tempEase;
				break;
			default:
				// nothing
				break;
			}
	}
	void ApplyFadeForRandom(){
		isFade = true ;
	}
	void ApplyMoveForRandom(){
		int whichMoveEffect = Random.Range( 1, 9 );
		switch(whichMoveEffect){
		case 1:
			isMoveFromBottom = true;
			break;
		case 2:
			isMoveFromLeft = true;
			break;
		case 3:
			isMoveFromRight = true;
			break;
		case 4:
			isMoveFromTop = true;
			break;
		case 5:
			isMoveFromBottomLeft = true;
			break;
		case 6:
			isMoveFromBottomRight = true;
			break;
		case 7:
			isMoveFromTopLeft = true;
			break;
		case 8:
			isMoveFromTopRight = true;
			break;
		}
	}
	void ApplyScaleForRandom(){
		int whichScaleEffect = Random.Range( 1, 3 );
		switch( whichScaleEffect ){
		case 1:
			isPopUp = true;
			break;
		case 2:
			isReversePopUp = true;
			break;
		}
	}
	void ApplyRotationForRandom(){
		int whichRotationEffect = Random.Range( 1, 4 );
		isRotation = true ;
		switch( whichRotationEffect ){
		case 1:
			axisX = true;
			break;
		case 2:
			axisY = true;
			break;
		case 3:
			axisZ = true;
			break;
		}
	}

	// dismiss global function
	public void DismissObjects(){
		if( dismissOnClick.Length > 0 ){
			for( int i = 0 ; i < dismissOnClick.Length ; i++ ){
				dismissOnClick[i].GetComponent<GUIEffects>().DismissNow();
			}
		}
	}

	// Dismiss local
	public void DismissNow(){
		if( isFade ){
			StartCoroutine( FadeOut() );
			Invoke( "DestroyMe" , fadeDismissDelay + ((2.0f/fadeSpeed) ) );
		}
		if( isMoveFromBottom ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromLeft ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromRight ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromTop ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromTopRight ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromTopLeft ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y + Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromBottomRight ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x + Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isMoveFromBottomLeft ){
			GUITween.MoveTo( gameObject , GUITween.Hash("position",new Vector3( gameObject.GetComponent<RectTransform>().localPosition.x - Screen.width , gameObject.GetComponent<RectTransform>().localPosition.y - Screen.height , gameObject.GetComponent<RectTransform>().localPosition.z ), "islocal", true, "time", 2.0f/positionEffectSpeed , "delay", positionEffectDismissDelay, "easeType", positionEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isPopUp ){
			GUITween.ScaleTo( gameObject , GUITween.Hash("scale", new Vector3( popUpStartValue, popUpStartValue, popUpStartValue ), "islocal", true, "time", 2.0f/scaleEffectSpeed, "delay", scaleEffectDismissDelay, "easeType", scaleEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isReversePopUp ){
			GUITween.ScaleTo( gameObject , GUITween.Hash("scale", new Vector3( reversePopUpStartValue, reversePopUpStartValue, reversePopUpStartValue ), "islocal", true, "time", 2.0f/scaleEffectSpeed, "delay", scaleEffectDismissDelay, "easeType", scaleEaseType, "ignoretimescale" , ignoreTimeScale ));
		}
		if( isRotation ){
			if( axisX ){
				rotationVector = new Vector3( -1.0f * flipsPerRotation, 0, 0 );
			}else
			if( axisY ){
				rotationVector = new Vector3( 0, -1.0f * flipsPerRotation, 0);
			}else
			if( axisZ ){
				rotationVector = new Vector3( 0, 0, -1.0f * flipsPerRotation );
			}else{
				rotationVector = new Vector3( flipsPerRotation, flipsPerRotation, flipsPerRotation );
				Debug.LogError("You have not selected your axis to rotate in GUIEffects. Please select any one axis in the inspector on your GUI element.");
			}
			if( isContinuosRotation ){
				GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", rotationDismissDelay, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateContinuos" ));
			}else{
				GUITween.RotateBy( gameObject, GUITween.Hash("amount", rotationVector, "time", 2.0f/rotationSpeed, "delay", rotationDismissDelay, "looptype", GUITween.LoopType.none, "easeType", rotationEaseType, "ignoretimescale" , ignoreTimeScale, "oncomplete", "WaitAndRotateNonContinuos" ));
			}
			Invoke( "DestroyMe" , rotationDismissDelay + rotationDismissTime );
		}



		// destroy
		if( isMoveFromBottom || isMoveFromLeft || isMoveFromRight || isMoveFromTop || isMoveFromBottomLeft || isMoveFromBottomRight || isMoveFromTopLeft || isMoveFromTopRight ){
			Invoke( "DestroyMe" , positionEffectDismissDelay + 2.0f/positionEffectSpeed );
		}
		if( isPopUp || isReversePopUp ){
			Invoke( "DestroyMe" , scaleEffectDismissDelay + 2.0f/scaleEffectSpeed );
		}
	}

	// destroy any one
	public void DestroyMe(){
		if( isDestroyOnDismiss ){
			Destroy( gameObject );
		}
	}

	// wait for fade out
	IEnumerator FadeOut(){
		yield return new WaitForSeconds( fadeDismissDelay );
		fadeCompleted = true ;
	}
}
