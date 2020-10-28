using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using System.Linq;
using Leap.Unity.Interaction;
using TMPro;
using UnityEngine.Serialization;


public class ReactionTimeCalcSnarc : MonoBehaviour
{
	
	/// <summary>
	/// LEAP HAND FINGER COUNTING PROJECT - SNARC
	/// [Umut Can Vural]
	/// [umutcan.vural@gmail.com]
	/// </summary>
	
	[FormerlySerializedAs("reactionTimer")] public float ReactionTimer;
    public float ReactionTimerInit; // initiation
	public bool IsTiming;
	[FormerlySerializedAs("subjectID")] public string SubjectId;
	
	private GameObject _buttonRight;
	private GameObject _buttonLeft;
	private GameObject _centerRight;
	private GameObject _centerLeft;

	private bool _isRightButtonActivated;
	private bool _isLeftButtonActivated;
	private bool _isReactButtonActivated;
	private bool _isResetButtonActivated;
	private bool _isCenterRightActivated;
	private bool _isCenterLeftActivated;
	private bool _isResetButtonReleased;
	
	private Controller _controller;
	private Frame _current;
	private Hand _handRight;
	private Hand _handLeft;
	private int _randomNumber;
	private const int NumberOfTrials = 15; //20*8 //TODO: Trial numbers adjustment
	private int _say;
	private int _blockNumber;
	private readonly List<string[]> _rowData = new List<string[]>();
	private string _textParity;
	private string _textCorrectSide;
	private string _textActivatedSide;
	private TextMeshPro _textMeshPro;
	private TextMesh _textMesh;
	
	private IEnumerator SnarcBlock01() //TODO: BLOCK1
	{
		var trialNumber = 0;
		const int blockNumber = 1; //normal snarc
		_blockNumber = blockNumber;
		
		var screenMain = GameObject.Find("ScreenMain").GetComponent<TextMeshPro>();
		screenMain.alignment = TextAlignmentOptions.Center;
		
		var rowDataTemp = new string[9];	
		rowDataTemp[0] = "Trial No";
		rowDataTemp[1] = "Subject ID";
		rowDataTemp[2] = "Reaction Time";
		rowDataTemp[3] = "Block No";
		rowDataTemp[4] = "Stimulus Number";
		rowDataTemp[5] = "Parity";
		rowDataTemp[6] = "Correct Response";
		rowDataTemp[7] = "Actual Response";
		rowDataTemp[8] = "Response Time";

		_rowData.Add(rowDataTemp);
		screenMain.text = "Practice";
		yield return new WaitUntil(() => _isResetButtonActivated);
		
		for (var k = 0; k < 2; k++)
		{
			var numbersSnarc = new List<int>() { 1,2,3,4,6,7,8,9 };
			var shuffledDigits = numbersSnarc.OrderBy(a => Guid.NewGuid()).ToList();
			
			for (var j = 0; j < 8; j++)
			{
				_randomNumber = shuffledDigits[j];
				var randomNumberStr = _randomNumber.ToString();
				
				screenMain.text = "+";
				yield return new WaitForSeconds(0.3f);
				screenMain.text = randomNumberStr;
				IsTiming = true;
				yield return new WaitUntil(() => _isResetButtonReleased);
				ReactionTimerInit = ReactionTimer;
				yield return new WaitUntil(() => _isReactButtonActivated);
				IsTiming = false;

				_textParity = _randomNumber % 2 == 0 ? "Even" : "Odd"; //Normal Snarc Rules
				_textCorrectSide = _textParity == "Even" ? "Right" : "Left";
				
				if (_textCorrectSide == _textActivatedSide)
				{
					screenMain.color = Color.green;
					screenMain.text = "TRUE!";
				}
				else
				{
					screenMain.color = Color.red;
					screenMain.text = "FALSE!";
				}
				yield return new WaitForSeconds(0.3f);
				screenMain.text = " ";
				screenMain.color = Color.white;
				ReactionTimer = 0;
				yield return new WaitUntil(() => _isResetButtonActivated);
			}
		}

		screenMain.text = "Block 1";
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
		yield return new WaitUntil(() => _isResetButtonActivated);
		
		for (var k = 0; k < NumberOfTrials; k++)
		{
			trialNumber += 1;
			var numbersSnarc = new List<int>() { 1,2,3,4,6,7,8,9 };
			var shuffledDigits = numbersSnarc.OrderBy(a => Guid.NewGuid()).ToList();
			for (var j = 0; j < 8; j++)
			{
				trialNumber += 1;
				_randomNumber = shuffledDigits[j];
				var randomNumberStr = _randomNumber.ToString();
				
				screenMain.text = "+";
				yield return new WaitForSeconds(0.3f);
				screenMain.text = randomNumberStr;
				IsTiming = true;
				yield return new WaitUntil(() => _isResetButtonReleased);
				ReactionTimerInit = ReactionTimer;
				yield return new WaitUntil(() => _isReactButtonActivated);
				IsTiming = false;

				_textParity = _randomNumber % 2 == 0 ? "Even" : "Odd";  //Normal Snarc Rules
				_textCorrectSide = _textParity == "Even" ? "Right" : "Left";
				
				if (_textCorrectSide == _textActivatedSide)
				{
					screenMain.color = Color.green;
					screenMain.text = "TRUE!";
				}
				else
				{
					screenMain.color = Color.red;
					screenMain.text = "FALSE!";
				}
				yield return new WaitForSeconds(0.3f);
				screenMain.text = " ";
				screenMain.color = Color.white;

				var initiationTime = ReactionTimer - ReactionTimerInit;
				
				rowDataTemp = new string[9];
				rowDataTemp[0] = "" + (trialNumber - 1) ;
				rowDataTemp[1] = "" + SubjectId;
				rowDataTemp[2] = "" + ReactionTimer;
				rowDataTemp[3] = "" + _blockNumber;
				rowDataTemp[4] = "" + _randomNumber;
				rowDataTemp[5] = "" + _textParity;
				rowDataTemp[6] = "" + _textCorrectSide;
				rowDataTemp[7] = "" + _textActivatedSide;
				rowDataTemp[8] = "" + initiationTime;
				_rowData.Add(rowDataTemp);
				Save();
				ReactionTimer = 0;
				yield return new WaitUntil(() => _isResetButtonActivated);
			}
		}

		yield return new WaitForSeconds(2f);
		screenMain.text = " END OF BLOCK 1 ";
	}

	private IEnumerator SnarcBlock02() //TODO: BLOCK2
	{
		var trialNumber = 0;
		const int blockNumber = 2; //reversed snarc
		_blockNumber = blockNumber;
		
		var screenMain = GameObject.Find("ScreenMain").GetComponent<TextMeshPro>();
		screenMain.alignment = TextAlignmentOptions.Center;
		var rowDataTemp = new string[9];	
		rowDataTemp[0] = "Trial No";
		rowDataTemp[1] = "Subject ID";
		rowDataTemp[2] = "Reaction Time";
		rowDataTemp[3] = "Block No";
		rowDataTemp[4] = "Stimulus Number";
		rowDataTemp[5] = "Parity";
		rowDataTemp[6] = "Correct Response";
		rowDataTemp[7] = "Actual Response";
		rowDataTemp[8] = "Response Time";
		
		_rowData.Add(rowDataTemp);
		screenMain.text = "Practice";
		yield return new WaitUntil(() => _isResetButtonActivated);
		
		for (var k = 0; k < 2; k++) 
		{
			var numbersSnarc = new List<int>() { 1,2,3,4,6,7,8,9 };
			var shuffledDigits = numbersSnarc.OrderBy(a => Guid.NewGuid()).ToList();
			for (var j = 0; j < 8; j++)
			{
				_randomNumber = shuffledDigits[j];
				var randomNumberStr = _randomNumber.ToString();
				
				screenMain.text = "+";
				yield return new WaitForSeconds(0.2f);
				screenMain.text = randomNumberStr;
				IsTiming = true;
				yield return new WaitUntil(() => _isResetButtonReleased);
				ReactionTimerInit = ReactionTimer;
				yield return new WaitUntil(() => _isReactButtonActivated);
				IsTiming = false;

				_textParity = _randomNumber % 2 == 0 ? "Even" : "Odd"; //Reversed Snarc Rules
				_textCorrectSide = _textParity == "Even" ? "Left" : "Right";
				
				if (_textCorrectSide == _textActivatedSide)
				{
					screenMain.color = Color.green;
					screenMain.text = "TRUE!";
				}
				else
				{
					screenMain.color = Color.red;
					screenMain.text = "FALSE!";
				}
				yield return new WaitForSeconds(0.3f);
				screenMain.text = " ";
				screenMain.color = Color.white;
				ReactionTimer = 0;
				yield return new WaitUntil(() => _isResetButtonActivated);
			}
		}

		screenMain.text = "Block 2";
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C)); // continue routine
		yield return new WaitUntil(() => _isResetButtonActivated);
		
		for (var k = 0; k < NumberOfTrials; k++)
		{
			trialNumber += 1;
			var numbersSnarc = new List<int>() { 1,2,3,4,6,7,8,9 };
			var shuffledDigits = numbersSnarc.OrderBy(a => Guid.NewGuid()).ToList();
			for (var j = 0; j < 8; j++)
			{
				trialNumber += 1;
				_randomNumber = shuffledDigits[j];
				var randomNumberStr = _randomNumber.ToString();
				
				screenMain.text = "+";
				yield return new WaitForSeconds(0.3f);
				screenMain.text = randomNumberStr;
				IsTiming = true;
				yield return new WaitUntil(() => _isResetButtonReleased);
				ReactionTimerInit = ReactionTimer;
				yield return new WaitUntil(() => _isReactButtonActivated);
				IsTiming = false;

				_textParity = _randomNumber % 2 == 0 ? "Even" : "Odd"; //Reversed Snarc Rules
				_textCorrectSide = _textParity == "Even" ? "Left" : "Right";
				
				if (_textCorrectSide == _textActivatedSide)
				{
					screenMain.color = Color.green;
					screenMain.text = "TRUE!";
				}
				else
				{
					screenMain.color = Color.red;
					screenMain.text = "FALSE!";
				}
				yield return new WaitForSeconds(0.3f);
				screenMain.text = " ";
				screenMain.color = Color.white;
				
				var initiationTime = ReactionTimer - ReactionTimerInit;
				
				rowDataTemp = new string[9];
				rowDataTemp[0] = "" + (trialNumber - 1) ;
				rowDataTemp[1] = "" + SubjectId;
				rowDataTemp[2] = "" + ReactionTimer;
				rowDataTemp[3] = "" + _blockNumber;
				rowDataTemp[4] = "" + _randomNumber;
				rowDataTemp[5] = "" + _textParity;
				rowDataTemp[6] = "" + _textCorrectSide;
				rowDataTemp[7] = "" + _textActivatedSide;
				rowDataTemp[8] = "" + initiationTime;
				_rowData.Add(rowDataTemp);
				Save();
				ReactionTimer = 0;
				yield return new WaitUntil(() => _isResetButtonActivated);
			}
		}

		yield return new WaitForSeconds(2f);
		screenMain.text = " END OF BLOCK 2 ";
	}

	private void Start() //TODO: START
	{
		_controller = new Controller();
		_current = _controller.Frame();
		GetComponent<Renderer>().material.color = Color.cyan;
		_textMeshPro = gameObject.AddComponent<TextMeshPro>();

		_buttonRight = GameObject.FindWithTag ("ButtonRight");
		_buttonLeft  = GameObject.FindWithTag ("ButtonLeft");
		_centerRight = GameObject.FindWithTag ("CenterRight");
		_centerLeft  = GameObject.FindWithTag ("CenterLeft");
	}

	private void Update()
	{
		if (IsTiming){ReactionTimer += Time.deltaTime;}
		
		if (_buttonRight.GetComponent<InteractionButton>().isPressed)
		{
			_isRightButtonActivated = true;
			_textActivatedSide = "Right";
		}
		else{_isRightButtonActivated = false;}
		
		if (_buttonLeft.GetComponent<InteractionButton>().isPressed)
		{
			_isLeftButtonActivated = true;
			_textActivatedSide = "Left";
		}
		else{_isLeftButtonActivated = false;}
		
		_isCenterRightActivated = _centerRight.GetComponent<InteractionButton>().isPressed;
		_isCenterLeftActivated = _centerLeft.GetComponent<InteractionButton>().isPressed;
		
		if (_isRightButtonActivated || _isLeftButtonActivated){_isReactButtonActivated = true;}
		else{_isReactButtonActivated = false;}
		
		if (_isCenterRightActivated && _isCenterLeftActivated){_isResetButtonActivated = true;}
		else{_isResetButtonActivated = false;}
		
		if (_isCenterRightActivated == false || _isCenterLeftActivated == false ){_isResetButtonReleased = true;}
		else{_isResetButtonReleased = false;}
		
		if (Input.GetKeyDown(KeyCode.N)) // Start normal routine
		{
			GetComponent<Renderer>().material.color = Color.grey;
			StartCoroutine(SnarcBlock01());
		}
		else if (Input.GetKeyDown(KeyCode.R)) // Start reversed routine
		{
			GetComponent<Renderer>().material.color = Color.grey;
			StartCoroutine(SnarcBlock02());
		}
		
	}

	public void Save()
	{
		var output = new string[_rowData.Count][];
		for (var i = 0; i < output.Length; i++)
		{
			output[i] = _rowData[i];
		}
		var length = output.GetLength(0);
		const string delimiter = ";";
		var sb = new StringBuilder();
		for (var index = 0; index < length; index++) sb.AppendLine(string.Join(delimiter, output[index]));
		var filePath = GetPath();
		var outStream = File.CreateText(filePath);
		outStream.WriteLine(sb);
		outStream.Close();
	}
	
	private string GetPath()
	{
		var fileName = "/CSV/" + "id_" + SubjectId + "_Block_" + _blockNumber + "_normaldata.csv";
			
#if UNITY_EDITOR
		return Application.dataPath + fileName;
#else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
	}
}
