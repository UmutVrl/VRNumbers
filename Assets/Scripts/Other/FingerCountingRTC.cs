using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;
using Leap.Unity;
using Debug = UnityEngine.Debug;
using System.Text;
using UnityEngine.Serialization;
using TMPro;

public class FingerCountingRTC : MonoBehaviour
{
	
	/// <summary>
	/// LEAP HAND FINGER COUNTING PROJECT
	/// 
	/// </summary>
	
	[FormerlySerializedAs("reactionTimer")] public float ReactionTimer;
	//	public float reactionTimer1;
//	public float reactionTimer2;

	[FormerlySerializedAs("IsTiming")] public bool isTiming = false;
	[FormerlySerializedAs("subjectID")] public string SubjectId;
	
	public RiggedHand Other;
	public RiggedFinger Other2;

	private Controller _controller;
	private Frame _current;
	private Leap.Hand _handRight;
	private Leap.Hand _handLeft;
	
	private TextMeshPro _textMeshPro;
	private TextMesh _textMesh;
	
	private int _randomNumber;
	private int _randomNumberSecond;
	private const int NumberOfTimesToRunFixDot = 2; 
	private const int NumberOfTimesToRunExperiment = 5;
	private const int NumberOfBlocks = 1;
	
	readonly string[] _digits = new string[10] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
	private List<string[]> rowData = new List<string[]>();
	private List<float> fingerRT = new List<float>();
	
	//TODO:
	//1- Create an Enum Practice - Congruent - Incongruity
	//2- Split trial Loops  TODO: DONE
	//3- Create run blocks  TODO: DONE
	//4- Posture type column (count - monitor) TODO: DONE
	//5- Target number TODO: DONE
	//6- Physical Distance ???
	//7- RTs TODO: DONE
	//8- LeftHand or RightHand ID
	//9- Finger ID
	//10- RTs of each finger ???
	//11- Time Gap for Trials ??? 3000ms
	//12- Catch Trial ???
	//13- Change output file name due to subject number
	
	
	private IEnumerator watcherCoroutine;

	private IEnumerator BeklemeCongruent() //TODO: Simplification of methods!!!!!!!!!!!!!!!
	{
		var trialNumber = 0;
		var blockNumber = 0;
		
		var screenMain = GameObject.Find("ScreenMain").GetComponent<TextMeshPro>();
		screenMain.alignment = TextAlignmentOptions.Center;
		
		
		var rowDataTemp = new string[15];	
		rowDataTemp[0] = "Trial No";
		rowDataTemp[1] = "Subject ID";
		rowDataTemp[2] = "Reaction Time";
		rowDataTemp[3] = "Step (A or B)";
		rowDataTemp[4] = "Target No";
		rowDataTemp[5] = "Block No";
		rowDataTemp[6] = "FixDot Dur";
		rowDataTemp[7] = "CatchTrial";
		rowDataTemp[8] = "CatchTrial Fail";
		rowDataTemp[9] = "IsPostureCounting";
		rowDataTemp[10] = "PhysDist";
		rowDataTemp[11] = "HandID";
		rowDataTemp[12] = "HandID RTs";
		rowDataTemp[13] = "FingerID";
		rowDataTemp[14] = "Finger RTs";
		
		rowData.Add(rowDataTemp);
		screenMain.text = "START";

		for (var k = 0; k < NumberOfBlocks; k++)
		{
			blockNumber += 1;
			
			for (var j = 0; j < NumberOfTimesToRunExperiment; j++)
			{
				
				const string stepA = "a";
				const string stepB = "b";
				
				_randomNumber = randomController();
				_randomNumberSecond = randomController();

			  Debug.Log("ILK" + _randomNumber + "IKI" + _randomNumberSecond);

				yield return new WaitForSeconds(2f);
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "+";
				screenMain.text = "+";
				var fixDotDuration = UnityEngine.Random.Range(0.5f, 1f); 
				yield return new WaitForSeconds(fixDotDuration);
		
				//TODO: step A-B should be different
				//TODO: OR should add fixDot to stepB
				//TODO: Thus each Trial is going to be Clone
				
//				for (var i = 0; i < NumberOfTimesToRunFixDot; ++i)
//				{
//					GameObject.Find("Numerator").GetComponent<TextMesh>().text = "+";
//					yield return new WaitForSeconds(0.1f);
//					GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
//					yield return new WaitForSeconds(0.1f);
//				}

				trialNumber += 1;
				yield return new WaitForSeconds(2f);
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = _digits[_randomNumber];
				screenMain.text = _digits[_randomNumber];
				var isPostureCounting = PostureDetector();
				isTiming = true;
				//TODO: to add reaction time of the first finger movement to the output file
				yield return new WaitUntil(() => !DigitDetector()[11].Equals(0.0f));
				fingerRT = DigitDetector(); //TODO: check this maybe no need to a new list 
				yield return new WaitUntil(() => (int) DigitDetector()[10] == _randomNumber + 1);
				isTiming = false;
				
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
				screenMain.color = Color.green;
				screenMain.text = "TRUE";
				yield return new WaitForSeconds(0.2f);
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
				screenMain.text = " ";
				screenMain.color = Color.white;
				

				foreach (var reactionTime in fingerRT)
				{
					Debug.Log(reactionTime);
				}
				
				rowDataTemp = new string[15];
				rowDataTemp[0] = "" + trialNumber;
				rowDataTemp[1] = "" + SubjectId;
				rowDataTemp[2] = "" + ReactionTimer;
				rowDataTemp[3] = "" + stepA;
				rowDataTemp[4] = "" + (_randomNumber + 1);
				rowDataTemp[5] = "" + blockNumber;
				rowDataTemp[6] = "" + fixDotDuration;
				rowDataTemp[7] = "no";
				rowDataTemp[8] = "";
				rowDataTemp[9] = "" + isPostureCounting;
				rowDataTemp[10] = "";
				rowDataTemp[11] = "";
				rowDataTemp[12] = "";
				rowDataTemp[13] = "";
				rowDataTemp[14] = "";
				rowData.Add(rowDataTemp);

				ReactionTimer = 0;

				trialNumber += 1;
				yield return new WaitForSeconds(2f);
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = _digits[_randomNumberSecond];
				screenMain.text = _digits[_randomNumberSecond];
				var isPostureCounting2 = PostureDetector();
				isTiming = true;
				
				yield return new WaitUntil(() => (int) DigitDetector()[10] == _randomNumberSecond + 1);
				isTiming = false;
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
				screenMain.color = Color.green;
				screenMain.text = "TRUE";
				yield return new WaitForSeconds(0.2f);
				//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
				screenMain.text = " ";
				screenMain.color = Color.white;
				
				
				rowDataTemp = new string[15];
				rowDataTemp[0] = "" + trialNumber;
				rowDataTemp[1] = "" + SubjectId;
				rowDataTemp[2] = "" + ReactionTimer;
				rowDataTemp[3] = "" + stepB;
				rowDataTemp[4] = "" + (_randomNumberSecond + 1);
				rowDataTemp[5] = "" + blockNumber;
				rowDataTemp[6] = "" + fixDotDuration;
				rowDataTemp[7] = "no";
				rowDataTemp[8] = "";
				rowDataTemp[9] = "" + isPostureCounting2;
				rowDataTemp[10] = "";
				rowDataTemp[11] = "";
				rowDataTemp[12] = "";
				rowDataTemp[13] = "";
				rowDataTemp[14] = "";
				rowData.Add(rowDataTemp);
				Save();

				ReactionTimer = 0;
			}
		}

		yield return new WaitForSeconds(2f);
		//GameObject.Find("Numerator").GetComponent<TextMesh>().text = "END OF EXPERIMENT";
		screenMain.text = "END OF EXPERIMENT";
	}

/*
	IEnumerator BeklemeIncongruent()
	{
		int randomNumber = UnityEngine.Random.Range(0, 4);
		int randomNumberSecond = UnityEngine.Random.Range(0, 4);

		yield return new WaitForSeconds(2.5f);
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumber];
		isTiming = true;
		//	GestureIncongruent();
		yield return new WaitUntil(() => DigitDetector() == randomNumber);
		Debug.Log(DigitDetector() + "  Reaction time: " + reactionTimer + " ms");
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
		yield return new WaitForSeconds(0.2f);
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
		isTiming = false;
		reactionTimer = 0;

		yield return new WaitForSeconds(2.5f);
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = digits[randomNumberSecond];
		isTiming = true;
		yield return new WaitUntil(() => DigitDetector() == randomNumberSecond);
		Debug.Log(DigitDetector() + "  Reaction timeSecond: " + reactionTimer + " ms");
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = "TRUE";
		yield return new WaitForSeconds(0.2f);
		GameObject.Find("Numerator").GetComponent<TextMesh>().text = "";
		isTiming = false;
		reactionTimer = 0;
	}
*/

	public bool PostureDetector()
	{
		
		_current = _controller.Frame();
		var rightHandPosture = new Vector3();
		var leftHandPosture = new Vector3();
		var isRHandPostureCounting = false;
		var isLHandPostureCounting = false;
		
		//TODO: is this part necessary for this method
		if (_current.Hands.Count == 1)
		{
			if (_current.Hands[0].IsRight)
			{
				_handRight = _current.Hands[0];
				_handLeft = null;
			}
			else if (_current.Hands[0].IsLeft)
			{
				_handLeft = _current.Hands[0];
				_handRight = null;
			}
		}
		else if (_current.Hands.Count == 2)
		{
			_handRight = _current.Hands[0];
			_handLeft = _current.Hands[1];
		}
		else if (_current.Hands.Count == 0)
		{
			_handRight = null;
			_handLeft = null;
		}

		
		if (_handRight != null)
		{
			rightHandPosture = _handRight.Basis.yBasis.ToVector3();
			
		}

		if (_handLeft != null)
		{
			leftHandPosture = _handLeft.Basis.yBasis.ToVector3();
		}

		if (rightHandPosture.y >= 0)
			isRHandPostureCounting = true;
		
		if (leftHandPosture.y >= 0)
			isLHandPostureCounting = true;
	
		return isLHandPostureCounting && isRHandPostureCounting;
	}

	List<float> DigitDetector() //Hand Digit Calculator
	{
		int a = 0, b = 0, c = 0, d = 0, e = 0, sum = 0;
		int f = 0, g = 0, h = 0, i = 0, j = 0;
		
		var _fingerRTs = new List<float>(){0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
		var rtSum = 0.0f;
		
		_current = _controller.Frame();
		
		if (_current.Hands.Count == 1)
		{
			if (_current.Hands[0].IsRight)
			{
				_handRight = _current.Hands[0];
				_handLeft = null;
			}
			else if (_current.Hands[0].IsLeft)
			{
				_handLeft = _current.Hands[0];
				_handRight = null;
			}
		}
		else if (_current.Hands.Count == 2)
		{
			_handRight = _current.Hands[0];
			_handLeft = _current.Hands[1];
		}
		else if (_current.Hands.Count == 0)
		{
			_handRight = null;
			_handLeft = null;
		}

		if (_handRight != null)
		{
			if (_handRight.Fingers[0].IsExtended)
			{
				a = 1;
				_fingerRTs[0] = ReactionTimer;
			}

			if (_handRight.Fingers[1].IsExtended)
			{
				b = 1;
				_fingerRTs[1] = ReactionTimer;
			}

			if (_handRight.Fingers[2].IsExtended)
			{
				c = 1;
				_fingerRTs[2] = ReactionTimer;
			}

			if (_handRight.Fingers[3].IsExtended)
			{
				d = 1;
				_fingerRTs[3] = ReactionTimer;
			}
			if (_handRight.Fingers[4].IsExtended)
			{
				e = 1;
				_fingerRTs[4] = ReactionTimer;
			}
			
		}

		if (_handLeft != null)
		{	

			if (_handLeft.Fingers[0].IsExtended)
			{
				f = 1;
				_fingerRTs[5] = ReactionTimer;
			}
			
			if (_handLeft.Fingers[1].IsExtended)
			{
				g = 1;
				_fingerRTs[6] = ReactionTimer;
			}

			if (_handLeft.Fingers[2].IsExtended)
			{
				h = 1;
				_fingerRTs[7] = ReactionTimer;
			}

			if (_handLeft.Fingers[3].IsExtended)
			{
				i = 1;
				_fingerRTs[8] = ReactionTimer;
			}

			if (_handLeft.Fingers[4].IsExtended)
			{
				j = 1;
				_fingerRTs[9] = ReactionTimer;
			}
		}
		sum = a + b + c + d + e + f + g + h + i + j;
		_fingerRTs[10] = sum;

		for(i = 0; i < 10; i++)
			rtSum = rtSum + _fingerRTs[i];      
		_fingerRTs[11] = rtSum;
		
		return _fingerRTs;
	}

/*
	List<float> FingerDetector() //TODO make list
	{
		var _fingerRTs = new List<float>(){0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};

		_current = _controller.Frame();
		
		
		if (_current.Hands.Count == 1)
		{
			if (_current.Hands[0].IsRight)
			{
				_handRight = _current.Hands[0];
				_handLeft = null;
			}
			else if (_current.Hands[0].IsLeft)
			{
				_handLeft = _current.Hands[0];
				_handRight = null;
			}
		}
		else if (_current.Hands.Count == 2)
		{
			_handRight = _current.Hands[0];
			_handLeft = _current.Hands[1];
		}
		else if (_current.Hands.Count == 0)
		{
			_handRight = null;
			_handLeft = null;
		}

		if (_handRight != null)
		{
			if (_handRight.Fingers[0].IsExtended)
				_fingerRTs[0] = ReactionTimer;
			if (_handRight.Fingers[1].IsExtended)
				_fingerRTs[1] = ReactionTimer;
			if (_handRight.Fingers[2].IsExtended)
				_fingerRTs[2] = ReactionTimer;
			if (_handRight.Fingers[3].IsExtended)
				_fingerRTs[3] = ReactionTimer;
			if (_handRight.Fingers[4].IsExtended)
				_fingerRTs[4] = ReactionTimer;
		}
		
		if (_handLeft != null)
		{
			if (_handLeft.Fingers[0].IsExtended)
				_fingerRTs[0] = ReactionTimer;
			if (_handLeft.Fingers[1].IsExtended)
				_fingerRTs[1] = ReactionTimer;
			if (_handLeft.Fingers[2].IsExtended)
				_fingerRTs[2] = ReactionTimer;
			if (_handLeft.Fingers[3].IsExtended)
				_fingerRTs[3] = ReactionTimer;
			if (_handLeft.Fingers[4].IsExtended)
				_fingerRTs[4] = ReactionTimer;
		}

		return _fingerRTs;
	}
*/
	

	void Start()
	{
		_controller = new Controller();
		_current = _controller.Frame();
		GetComponent<Renderer>().material.color = Color.cyan;
	}

	// Update is called once per frame
	void Update()
	{
		if (isTiming){ReactionTimer += Time.deltaTime;}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			GetComponent<Renderer>().material.color = Color.grey;
			StartCoroutine(BeklemeCongruent());
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			GetComponent<Renderer>().material.color = Color.grey;
		//	StartCoroutine(BeklemeIncongruent());
		}
	}

	int randomController() //TODO: check proper randomness
	{
		var numberRandom = UnityEngine.Random.Range (0, 10);
		var previous = numberRandom;
		while (numberRandom == previous)
		{
			numberRandom = UnityEngine.Random.Range(0, 10);
		}
		return numberRandom;
	}

//	public void FileUtility()
//	{
//		rowDataTemp = new string[5];
//		rowDataTemp[0] = "" + j; // ID
//		rowDataTemp[1] = "" + reactionTimer1;
//		rowDataTemp[2] = "" + reactionTimer2;
//		rowDataTemp[3] = "" + subjectID;
//		rowDataTemp[4] = "" + randomNumber + 1;
//		rowData.Add(rowDataTemp);
//		
//	}
	

	public void Save()
	{
		var output = new string[rowData.Count][];
		
		for (var i = 0; i < output.Length; i++)
		{
			output[i] = rowData[i];
		}
		var length = output.GetLength(0);
		const string delimiter = ";";
		var sb = new StringBuilder();
		for (var index = 0; index < length; index++) sb.AppendLine(string.Join(delimiter, output[index]));
		var filePath = getPath();
		var outStream = System.IO.File.CreateText(filePath);
		outStream.WriteLine(sb);
		outStream.Close();
	}

	private string getPath()
	{
#if UNITY_EDITOR
		return Application.dataPath + "/CSV/" + "Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
	}
}
