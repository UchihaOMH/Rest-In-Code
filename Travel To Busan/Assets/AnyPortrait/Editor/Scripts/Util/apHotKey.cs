﻿/*
*	Copyright (c) 2017-2020. RainyRizzle. All rights reserved
*	Contact to : https://www.rainyrizzle.com/ , contactrainyrizzle@gmail.com
*
*	This file is part of [AnyPortrait].
*
*	AnyPortrait can not be copied and/or distributed without
*	the express perission of [Seungjik Lee].
*
*	Unless this file is downloaded from the Unity Asset Store or RainyRizzle homepage, 
*	this file and its users are illegal.
*	In that case, the act may be subject to legal penalties.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using System.Diagnostics;


using AnyPortrait;

namespace AnyPortrait
{
	

	/// <summary>
	/// 에디터의 단축키를 처리하는 객체.
	/// 단축키 처리는 OnGUI이 후반부에 해야하는데,
	/// UI별로 단축키에 대한 처리 요구가 임의의 위치에서 이루어지므로, 이를 대신 받아서 지연시키는 객체.
	/// 모든 함수 요청은 OnGUI마다 리셋되고 다시 받는다.
	/// 이벤트에 따라 묵살될 수 있다.
	/// </summary>
	public class apHotKey
	{
		public delegate void FUNC_HOTKEY_EVENT(object paramObject);
		public delegate void FUNC_RESV_HOTKEY_EVENT(KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, object paramObject);

		//변경 20.1.26 : Label을 string으로 받지 말고 enum으로 받은뒤, 미리 정의된 Label을 출력하자
		public enum LabelText
		{
			None,
			ToggleWorkspaceSize,
			Select,
			Move,
			Rotate,
			Scale,
			OnionSkinToggle,
			ChangeBoneVisiblity,
			IncreaseBrushSize,
			DecreaseBrushSize,
			RemovePolygon,
			SelectAllVertices,
			RemoveVertices,
			ToggleEditingMode,
			ToggleSelectionLock,
			ToggleModifierLock,
			ToggleLayerLock,
			AddNewKeyframe,
			RemoveKeyframe,
			RemoveKeyframes,
			PlayPause,
			PreviousFrame,
			NextFrame,
			FirstFrame,
			LastFrame,
			CopyKeyframes,
			PasteKeyframes,
			IncreaseBrushRadius,
			DecreaseBrushRadius,
			BrushMode_Add,
			BrushMode_Multiply,
			BrushMode_Blur,
			IncreaseBrushIntensity,
			DecreaseBrushIntensity,

			//TODO
		}

		// Unit Class
		public class HotKeyEvent
		{
			public KeyCode _keyCode;
			//public string _label;//이전
			public LabelText _labelType = LabelText.None;
			public bool _isShift;
			public bool _isAlt;
			public bool _isCtrl;
			public object _paramObject;
			public FUNC_HOTKEY_EVENT _funcEvent;
			public bool _isCombination;

			public HotKeyEvent()
			{
				_keyCode = KeyCode.Space;
				//_label;
				_isShift = false;
				_isAlt = false;
				_isCtrl = false;
				_paramObject = null;
				_funcEvent = null;
				_isCombination = false;
			}

			public void SetEvent(FUNC_HOTKEY_EVENT funcEvent, 
				//string label, 
				LabelText labelType,
				KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, object paramObject)
			{
				_funcEvent = funcEvent;
				//_label = label;//이전
				_labelType = labelType;//변경 20.1.26
				_keyCode = keyCode;
				_isShift = isShift;
				_isAlt = isAlt;
				_isCtrl = isCtrl;

				_isCombination = _isShift || _isAlt || _isCtrl;

				_paramObject = paramObject;
			}
		}

		//추가 20.1.26 : 특정 단축키는 미리 저장했다가 사용한다.
		//복수개의 키 타입을 한개의 이벤트에 매핑할 수 있다.
		//KeyCode가 아닌 별도의 Enum을 이용한다.
		//PopEvent가 아니며, 고정 크기의 배열을 이용한다. (최대 개수가 정해져있음)
		//이 키들은 일반적이므로 Label이 없다.
		public enum RESERVED_KEY
		{
			None,
			Arrow,
			Arrow_Shift,
			Arrow_Ctrl,
			EnterOrEscape,
			Escape,
			Enter,
		}
		public class ReservedHotKeyEvent
		{
			public RESERVED_KEY _reservedHotkey = RESERVED_KEY.None;
			public bool _isShift = false;
			public bool _isCtrl = false;
			public bool _isAlt = false;
			public bool _isCombination = false;

			public FUNC_RESV_HOTKEY_EVENT _funcEvent = null;
			public object _paramObject = null;

			public bool _isRegistered = false;

			public ReservedHotKeyEvent(RESERVED_KEY keyType, bool isShift, bool isCtrl, bool isAlt)
			{
				_reservedHotkey = keyType;
				_isShift = isShift;
				_isCtrl = isCtrl;
				_isAlt = isAlt;

				_isCombination = _isShift || _isCtrl || _isAlt;

				_funcEvent = null;
				_paramObject = null;

				_isRegistered = false;
			}

			public void SetEvent(FUNC_RESV_HOTKEY_EVENT funcEvent, object paramObject)
			{
				_isRegistered = true;
				_funcEvent = funcEvent;
				_paramObject = paramObject;
			}

			public void ClearEvent()
			{
				_funcEvent = null;
				_paramObject = null;
				_isRegistered = false;
			}
		}


		//특수키의 딜레이 처리
		//특수키는 Up 이벤트 발생 후에도 아주 짧은 시간(0.3초)동안 Down 이벤트로 기록해야한다.
		public enum SPECIAL_KEY
		{
			Ctrl, Shift, Alt
		}
		private class SpecialKeyProcess
		{
			public SPECIAL_KEY _key;
			public bool _isPressed_Input;
			public bool _isPressed_Delayed;
			public System.Diagnostics.Stopwatch _timer = new System.Diagnostics.Stopwatch();

			private const long DELAY_TIME_MSEC = 300;//0.3초

			public SpecialKeyProcess(SPECIAL_KEY specialKey)
			{
				_key = specialKey;
				_isPressed_Input = false;
				_isPressed_Delayed = false;
			}

			public void OnKeyDown()
			{
				_isPressed_Input = true;
				_isPressed_Delayed = true;
			}

			public void OnKeyUp()
			{
				if (_isPressed_Input)
				{
					_isPressed_Input = false;
					_isPressed_Delayed = true;//일단은 True

					_timer.Reset();
					_timer.Start();
				}
			}

			public bool IsPressed()
			{
				if(_isPressed_Input)
				{
					//실제로 눌린 상태
					return true;
				}
				else
				{
					if(_isPressed_Delayed)
					{
						//일단은 딜레이 중이다.
						if(_timer.ElapsedMilliseconds > DELAY_TIME_MSEC)
						{
							_isPressed_Delayed = false;
							_timer.Stop();
						}
						//else
						//{
						//	Debug.Log("딜레이된 특수 키 : " + _key + " (" + (float)(_timer.ElapsedMilliseconds / 1000.0) + "s)");
						//}
					}

					return _isPressed_Delayed;

				}
			}

			public void ResetTimer()
			{
				if(!_isPressed_Input)
				{
					_isPressed_Delayed = false;
					_timer.Reset();
					_timer.Stop();
				}
			}
		}

		public enum EVENT_RESULT
		{
			None, NormalEvent, ReservedEvent
		}

		// Members
		//---------------------------------------------
		private int _iEvent = 0;
		private const int NUM_INIT_EVENT_POOL = 20;
		private List<HotKeyEvent> _hotKeyEvents_Pool = new List<HotKeyEvent>();
		private List<HotKeyEvent> _hotKeyEvents_Live = new List<HotKeyEvent>();

		//추가 20.1.27 : Reserved HotKey를 추가
		private Dictionary<RESERVED_KEY, ReservedHotKeyEvent> _reservedHotKeyEvents_Mapping = new Dictionary<RESERVED_KEY, ReservedHotKeyEvent>();
		private List<ReservedHotKeyEvent> _reservedHotKeyEvents = new List<ReservedHotKeyEvent>();
		private int _nReservedHotKeyEvent = 0;


		private bool _isAnyEvent_Normal = false;
		private bool _isAnyEvent_Reserved = false;


		private KeyCode _prevKey = KeyCode.None;

		private Dictionary<SPECIAL_KEY, SpecialKeyProcess> _specialKeys = new Dictionary<SPECIAL_KEY, SpecialKeyProcess>();


		//추가 20.1.26 : 최적화 코드
		//Label을 String 타입으로 받지 말고 미리 만든뒤 재활용하자
		private Dictionary<LabelText, apStringWrapper> _labels = new Dictionary<LabelText, apStringWrapper>();

		//추가 20.1.27 : 입력 연산 후 바로 리턴하지 말고, 결과값을 변수로 가지고 있자
		private HotKeyEvent _resultHotKeyEvent = null;
		private ReservedHotKeyEvent _resultReservedHotKeyEvent = null;

		// Init
		//---------------------------------------------
		public apHotKey()
		{
			_isAnyEvent_Normal = false;
			_isAnyEvent_Reserved = false;

			_resultHotKeyEvent = null;
			_resultReservedHotKeyEvent = null;


			if(_hotKeyEvents_Pool == null)
			{
				_hotKeyEvents_Pool = new List<HotKeyEvent>();
			}
			if(_hotKeyEvents_Live == null)
			{
				_hotKeyEvents_Live = new List<HotKeyEvent>();
			}
			_hotKeyEvents_Pool.Clear();
			_hotKeyEvents_Live.Clear();

			for (int i = 0; i < NUM_INIT_EVENT_POOL; i++)
			{
				_hotKeyEvents_Pool.Add(new HotKeyEvent());
			}
			_iEvent = 0;


			//추가 20.1.27 : 예약된 시스템 단축키 초기화
			InitReservedHotKeys();


			//_isLock = false;
			//_isSpecialKey_Ctrl = false;
			//_isSpecialKey_Shift = false;
			//_isSpecialKey_Alt = false;
			_prevKey = KeyCode.None;

			if(_specialKeys == null)
			{
				_specialKeys = new Dictionary<SPECIAL_KEY, SpecialKeyProcess>();
			}
			_specialKeys.Clear();

			_specialKeys.Add(SPECIAL_KEY.Ctrl, new SpecialKeyProcess(SPECIAL_KEY.Ctrl));
			_specialKeys.Add(SPECIAL_KEY.Alt, new SpecialKeyProcess(SPECIAL_KEY.Alt));
			_specialKeys.Add(SPECIAL_KEY.Shift, new SpecialKeyProcess(SPECIAL_KEY.Shift));

			InitLabels();
		}




		//추가 : 단축키 Label을 여기서 만들자
		private void InitLabels()
		{
			if(_labels == null)
			{
				_labels = new Dictionary<LabelText, apStringWrapper>();
			}
			_labels.Clear();

			AddLabelText(LabelText.None, "None");
			AddLabelText(LabelText.ToggleWorkspaceSize, "Toggle Workspace Size");
			AddLabelText(LabelText.Select, "Select");
			AddLabelText(LabelText.Move, "Move");
			AddLabelText(LabelText.Rotate, "Rotate");
			AddLabelText(LabelText.Scale, "Scale");
			AddLabelText(LabelText.OnionSkinToggle, "Onion Skin Toggle");
			AddLabelText(LabelText.ChangeBoneVisiblity, "Change Bone Visiblity");
			AddLabelText(LabelText.IncreaseBrushSize, "Increase Brush Size");
			AddLabelText(LabelText.DecreaseBrushSize, "Decrease Brush Size");
			AddLabelText(LabelText.RemovePolygon, "Remove Polygon");
			AddLabelText(LabelText.SelectAllVertices, "Select All Vertices");
			AddLabelText(LabelText.RemoveVertices, "Remove Vertices");
			AddLabelText(LabelText.ToggleEditingMode, "Toggle Editing Mode");
			AddLabelText(LabelText.ToggleSelectionLock, "Toggle Selection Lock");
			AddLabelText(LabelText.ToggleModifierLock, "Toggle Modifier Lock");
			AddLabelText(LabelText.ToggleLayerLock,"Toggle Layer Lock");
			AddLabelText(LabelText.AddNewKeyframe, "Add New Keyframe");
			AddLabelText(LabelText.RemoveKeyframe, "Remove Keyframe");
			AddLabelText(LabelText.RemoveKeyframes, "Remove Keyframes");
			AddLabelText(LabelText.PlayPause, "Play/Pause");
			AddLabelText(LabelText.PreviousFrame, "Previous Frame");
			AddLabelText(LabelText.NextFrame, "Next Frame");
			AddLabelText(LabelText.FirstFrame, "First Frame");
			AddLabelText(LabelText.LastFrame, "Last Frame");
			AddLabelText(LabelText.CopyKeyframes, "Copy Keyframes");
			AddLabelText(LabelText.PasteKeyframes, "Paste Keyframes");
			AddLabelText(LabelText.IncreaseBrushRadius, "Increase Brush Radius");
			AddLabelText(LabelText.DecreaseBrushRadius, "Decrease Brush Radius");
			AddLabelText(LabelText.BrushMode_Add, "Brush Mode - Add");
			AddLabelText(LabelText.BrushMode_Multiply, "Brush Mode - Multiply");
			AddLabelText(LabelText.BrushMode_Blur, "Brush Mode - Blur");
			AddLabelText(LabelText.IncreaseBrushIntensity, "Increase Brush Intensity");
			AddLabelText(LabelText.DecreaseBrushIntensity, "Decrease Brush Intensity");
			
			//TODO : Label을 추가합니다.
		}

		private void AddLabelText(LabelText labelType, string text)
		{
			_labels.Add(labelType, apStringWrapper.MakeStaticText(text));
		}


		private void InitReservedHotKeys()
		{
			if(_reservedHotKeyEvents_Mapping == null)
			{
				_reservedHotKeyEvents_Mapping = new Dictionary<RESERVED_KEY, ReservedHotKeyEvent>();
			}
			if(_reservedHotKeyEvents == null)
			{
				_reservedHotKeyEvents = new List<ReservedHotKeyEvent>();
			}
			
			_reservedHotKeyEvents_Mapping.Clear();
			_reservedHotKeyEvents.Clear();

			AddReservedHotKeyEvent(RESERVED_KEY.None, false, false, false);
			AddReservedHotKeyEvent(RESERVED_KEY.Arrow, false, false, false);
			AddReservedHotKeyEvent(RESERVED_KEY.Arrow_Shift, true, false, false);
			AddReservedHotKeyEvent(RESERVED_KEY.Arrow_Ctrl, false, true, false);
			AddReservedHotKeyEvent(RESERVED_KEY.EnterOrEscape, false, false, false);
			AddReservedHotKeyEvent(RESERVED_KEY.Escape, false, false, false);
			AddReservedHotKeyEvent(RESERVED_KEY.Enter, false, false, false);
			

			//TODO : 만약 특수 시스템키가 추가되면 여기에 더 추가하자
			
			
			_nReservedHotKeyEvent = _reservedHotKeyEvents.Count;
		}

		private void AddReservedHotKeyEvent(RESERVED_KEY hotKeyType, bool isShift, bool isCtrl, bool isAlt)
		{
			ReservedHotKeyEvent newHotKeyEvent = new ReservedHotKeyEvent(hotKeyType, isShift, isCtrl, isAlt);
			newHotKeyEvent.ClearEvent();
			_reservedHotKeyEvents.Add(newHotKeyEvent);
			_reservedHotKeyEvents_Mapping.Add(hotKeyType, newHotKeyEvent);
		}


		/// <summary>
		/// OnGUI 초기에 호출해주자
		/// </summary>
		public void Clear()
		{
			if (_isAnyEvent_Normal)
			{
				_isAnyEvent_Normal = false;
				_hotKeyEvents_Live.Clear();
				_iEvent = 0;
			}

			if(_isAnyEvent_Reserved)
			{
				_isAnyEvent_Reserved = false;
				for (int i = 0; i < _nReservedHotKeyEvent; i++)
				{
					_reservedHotKeyEvents[i].ClearEvent();
				}
			}

			_resultHotKeyEvent = null;
			_resultReservedHotKeyEvent = null;
		}


		// Input Event
		//------------------------------------------------------------------------------
		//이전 : 바로 결과 HotKeyEvent 리턴
		//public apHotKey.HotKeyEvent OnKeyEvent(KeyCode keyCode, bool isCtrl, bool isShift, bool isAlt, bool isPressed)
		
		//변경 20.1.27 : 리턴값이 바로 안나오고, GetResultEvent / GetReservedResultEvent 함수로 가져오게 변경 (뭔가 결과가 발생하는지 체크 위한 bool 리턴)
		public EVENT_RESULT OnKeyEvent(KeyCode keyCode, bool isCtrl, bool isShift, bool isAlt, bool isPressed)
		{
			if(keyCode == KeyCode.None)
			{
				return EVENT_RESULT.None;
			}
			
			
			if (isPressed)
			{
				//Pressed 이벤트의 경우, 너무 잦은 이벤트 호출이 문제다.
				//if (_isLock)
				//{
				//	Debug.LogWarning("키 입력 되었으나 Lock [" + keyCode + "]");
				//	return null;
				//}

				if (_prevKey == keyCode)
				{
					//Debug.Log(">> 이전 키와 같음 : " + keyCode);
					return EVENT_RESULT.None;
				}

				_prevKey = keyCode;
			}
			else
			{
				_prevKey = KeyCode.None;
			}

			//추가적으로, 유니티에서 제공하는 값에 따라서도 변동
			if (isPressed)
			{
				//Pressed인 경우 False > True로만 보정
				if (isCtrl)
				{
					_specialKeys[SPECIAL_KEY.Ctrl].OnKeyDown();
				}
				if (isShift)
				{
					_specialKeys[SPECIAL_KEY.Shift].OnKeyDown();
				}
				if (isAlt)
				{
					_specialKeys[SPECIAL_KEY.Alt].OnKeyDown();
				}
			}
			else
			{
				//Released인 경우 False > True로만 보정
				if (!isCtrl)
				{
					_specialKeys[SPECIAL_KEY.Ctrl].OnKeyUp();
				}
				if (!isShift)
				{
					_specialKeys[SPECIAL_KEY.Shift].OnKeyUp();
				}
				if (!isAlt)
				{
					_specialKeys[SPECIAL_KEY.Alt].OnKeyUp();
				}
			}

			switch (keyCode)
			{		
				// Special Key
#if UNITY_EDITOR_OSX
				case KeyCode.LeftCommand:
				case KeyCode.RightCommand:
#else
				case KeyCode.LeftControl:
				case KeyCode.RightControl:
#endif
					if(isPressed)
					{
						_specialKeys[SPECIAL_KEY.Ctrl].OnKeyDown();
					}
					else
					{
						_specialKeys[SPECIAL_KEY.Ctrl].OnKeyUp();
					}
					break;

				case KeyCode.LeftShift:
				case KeyCode.RightShift:
					if(isPressed)
					{
						_specialKeys[SPECIAL_KEY.Shift].OnKeyDown();
					}
					else
					{
						_specialKeys[SPECIAL_KEY.Shift].OnKeyUp();
					}
					break;

				case KeyCode.LeftAlt:
				case KeyCode.RightAlt:
					if(isPressed)
					{
						_specialKeys[SPECIAL_KEY.Alt].OnKeyDown();
					}
					else
					{
						_specialKeys[SPECIAL_KEY.Alt].OnKeyUp();
					}
					break;

				default:
					//그 외의 키값이라면..
					//Up 이벤트에만 반응하자 > 변경
					//특수키가 있는 단축키 => Up 이벤트에서만 적용
					//특수키가 없는 단축키 => Down 이벤트에서만 적용
					//if (!isPressed)
					{
						//해당하는 이벤트가 있는가?
						//추가 20.1.27 : ReservedHotKeyEvent 먼저 체크한다.
						if (_isAnyEvent_Reserved)
						{
							ReservedHotKeyEvent reservedHotKeyEvent = CheckReservedHotKeyEvent(keyCode,
																			_specialKeys[SPECIAL_KEY.Shift].IsPressed(),
																			_specialKeys[SPECIAL_KEY.Alt].IsPressed(),
																			_specialKeys[SPECIAL_KEY.Ctrl].IsPressed(),
																			isPressed);

							if(reservedHotKeyEvent != null)
							{
								//Reserved 이벤트가 먼저 발생했다.
								_resultReservedHotKeyEvent = reservedHotKeyEvent;
							}
						}

						if (_resultReservedHotKeyEvent == null && _isAnyEvent_Normal)
						{
							//Reserved 이벤트가 발생하지 않고, 기본 이벤트가 등록되었다면
							HotKeyEvent hotkeyEvent = CheckHotKeyEvent(keyCode,
																			_specialKeys[SPECIAL_KEY.Shift].IsPressed(),
																			_specialKeys[SPECIAL_KEY.Alt].IsPressed(),
																			_specialKeys[SPECIAL_KEY.Ctrl].IsPressed(),
																			isPressed);

							if (hotkeyEvent != null)
							{
								//이벤트가 발생했다.
								_resultHotKeyEvent = hotkeyEvent;
								//////일단 이 메인 키를 누른 상태에서 Lock을 건다.
								////_isLock = true;
								//return hotkeyEvent;
							}
						}

						

						//딜레이 처리가 끝났다면 특수키 타이머를 리셋한다.
						_specialKeys[SPECIAL_KEY.Shift].ResetTimer();
						_specialKeys[SPECIAL_KEY.Alt].ResetTimer();
						_specialKeys[SPECIAL_KEY.Ctrl].ResetTimer();

						
					}
					
					break;
			}
			if(_resultReservedHotKeyEvent != null)
			{
				return EVENT_RESULT.ReservedEvent;
			}
			if(_resultHotKeyEvent != null)
			{
				return EVENT_RESULT.NormalEvent;
			}
			return EVENT_RESULT.None;
		}

		public HotKeyEvent GetResultEvent()
		{
			return _resultHotKeyEvent;
		}

		public ReservedHotKeyEvent GetResultReservedEvent()
		{
			return _resultReservedHotKeyEvent;
		}
		
			



		#region [미사용 코드]
		//		public apHotKey.HotKeyEvent OnKeyDown(KeyCode keyCode, bool isCtrl, bool isShift, bool isAlt)
		//		{
		//			Debug.Log("OnKeyDown : " + keyCode);
		//			if(keyCode == KeyCode.None)
		//			{
		//				return null;
		//			}

		//			if(_isLock)
		//			{
		//				Debug.LogWarning("키 입력 되었으나 Lock [" + keyCode + "]");
		//				return null;
		//			}

		//			if(_prevKey == keyCode)
		//			{
		//				Debug.Log(">> 이전 키와 같음 : " + keyCode);
		//				return null;
		//			}

		//			_prevKey = keyCode;

		//			Debug.LogWarning("Key Down : " + keyCode);


		//			//추가적으로, 유니티에서 제공하는 값에 따라서도 변동
		//			if(isCtrl)
		//			{
		//				_isSpecialKey_Ctrl = true;
		//			}
		//			if(isShift)
		//			{
		//				_isSpecialKey_Shift = true;
		//			}
		//			if (isAlt)
		//			{
		//				_isSpecialKey_Alt = true;
		//			}

		//			switch (keyCode)
		//			{		
		//				// Special Key
		//#if UNITY_EDITOR_OSX
		//				case KeyCode.LeftCommand:
		//				case KeyCode.RightCommand:
		//#else
		//				case KeyCode.LeftControl:
		//				case KeyCode.RightControl:
		//#endif
		//					_isSpecialKey_Ctrl = true;
		//					break;

		//				case KeyCode.LeftShift:
		//				case KeyCode.RightShift:
		//					_isSpecialKey_Shift = true;
		//					break;

		//				case KeyCode.LeftAlt:
		//				case KeyCode.RightAlt:
		//					_isSpecialKey_Alt = true;
		//					break;

		//				default:
		//					//그 외의 키값이라면..
		//					_mainKey = keyCode;

		//					//해당하는 이벤트가 있는가?
		//					apHotKey.HotKeyEvent hotkeyEvent = CheckHotKeyEvent(_mainKey, _isSpecialKey_Shift, _isSpecialKey_Alt, _isSpecialKey_Ctrl);
		//					if(hotkeyEvent != null)
		//					{
		//						//일단 이 메인 키를 누른 상태에서 Lock을 건다.
		//						_isLock = true;

		//						return hotkeyEvent;
		//					}
		//					break;
		//			}

		//			return null;

		//		}

		//		public void OnKeyUp(KeyCode keyCode, bool isCtrl, bool isShift, bool isAlt)
		//		{
		//			if(keyCode == KeyCode.None)
		//			{
		//				return;
		//			}

		//			//추가적으로, 유니티에서 제공하는 값에 따라서도 변동 (False로만)
		//			if(!isCtrl)
		//			{
		//				_isSpecialKey_Ctrl = false;
		//			}
		//			if(!isShift)
		//			{
		//				_isSpecialKey_Shift = false;
		//			}
		//			if (!isAlt)
		//			{
		//				_isSpecialKey_Alt = false;
		//			}

		//			Debug.LogError("Key Up : " + keyCode);
		//			_prevKey = KeyCode.None;

		//			//Lock을 풀 수 있을까
		//			switch (keyCode)
		//			{		
		//				// Special Key
		//#if UNITY_EDITOR_OSX
		//				case KeyCode.LeftCommand:
		//				case KeyCode.RightCommand:
		//#else
		//				case KeyCode.LeftControl:
		//				case KeyCode.RightControl:
		//#endif
		//					_isSpecialKey_Ctrl = false;
		//					break;

		//				case KeyCode.LeftShift:
		//				case KeyCode.RightShift:
		//					_isSpecialKey_Shift = false;
		//					break;

		//				case KeyCode.LeftAlt:
		//				case KeyCode.RightAlt:
		//					_isSpecialKey_Alt = false;
		//					break;

		//				default:
		//					if(keyCode == _mainKey)
		//					{
		//						//Lock을 풀자
		//						_isLock = false;

		//						Debug.Log("[" + _mainKey + "] 단축키 Lock 해제됨");
		//						_mainKey = KeyCode.ScrollLock;
		//					}
		//					break;
		//			}

		//		} 
		#endregion

		// Functions
		//------------------------------------------------------------------------------
		//1) 일반 단축키의 Add, Pop, Check 함수들
		public void AddHotKeyEvent(FUNC_HOTKEY_EVENT funcEvent, LabelText labelType, KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, object paramObject)
		{
			_hotKeyEvents_Live.Add(PopEvent(funcEvent, labelType, keyCode, isShift, isAlt, isCtrl, paramObject));
			_isAnyEvent_Normal = true;
		}

		// 변경 3.25 : 매번 생성하는 방식에서 Pop 방식으로 변경
		private HotKeyEvent PopEvent(FUNC_HOTKEY_EVENT funcEvent, LabelText labelType, KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, object paramObject)
		{
			if(_iEvent >= _hotKeyEvents_Pool.Count)
			{
				//두개씩 늘리자
				for (int i = 0; i < 2; i++)
				{
					_hotKeyEvents_Pool.Add(new HotKeyEvent());
				}

				//Debug.Log("입력 풀 부족 : " + _hotKeyEvents_Pool.Count + " [" + label + "]");
			}

			HotKeyEvent result = _hotKeyEvents_Pool[_iEvent];
			_iEvent++;
			result.SetEvent(funcEvent, labelType, keyCode, isShift, isAlt, isCtrl, paramObject);
			return result;
		}

		/// <summary>
		/// OnGUI 후반부에 체크해준다.
		/// Event가 used가 아니라면 호출 가능
		/// </summary>
		/// <param name=""></param>
		public HotKeyEvent CheckHotKeyEvent(KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, bool isPressed)
		{
			if (!_isAnyEvent_Normal)
			{
				return null;
			}

			
			HotKeyEvent hkEvent = null;
			for (int i = 0; i < _hotKeyEvents_Live.Count; i++)
			{
				hkEvent = _hotKeyEvents_Live[i];

				//Pressed 이벤트 = 단일 키
				//Released 이벤트 = 조합 키
				// 위조건이 안맞으면 continue
				if((isPressed && hkEvent._isCombination)
					|| (!isPressed && !hkEvent._isCombination))
				{
					//조합키인데 Pressed 이벤트이거나
					//단일키인데 Released 이벤트라면
					//패스
					continue;
				}

				if (hkEvent._keyCode == keyCode &&
					hkEvent._isShift == isShift &&
					hkEvent._isAlt == isAlt &&
					hkEvent._isCtrl == isCtrl)
				{
					try
					{
						//저장된 이벤트를 실행하자
						hkEvent._funcEvent(hkEvent._paramObject);

						return hkEvent;
					}
					catch (Exception ex)
					{
						Debug.LogError("HotKey Event Exception : " + ex);
						return null;
					}
				}
			}
			return null;
		}


		//2) 예약된 단축키의 Add, Check
		public void AddReservedHotKey(FUNC_RESV_HOTKEY_EVENT funcEvent, RESERVED_KEY keyType, object paramObject)
		{
			_reservedHotKeyEvents_Mapping[keyType].SetEvent(funcEvent, paramObject);
			_isAnyEvent_Reserved = true;
		}

		public ReservedHotKeyEvent CheckReservedHotKeyEvent(KeyCode keyCode, bool isShift, bool isAlt, bool isCtrl, bool isPressed)
		{
			if(!_isAnyEvent_Reserved)
			{
				return null;
			}

			ReservedHotKeyEvent hkEvent = null;
			bool isValidInput = false;
			for (int i = 0; i < _nReservedHotKeyEvent; i++)
			{
				hkEvent = _reservedHotKeyEvents[i];
				
				if(!hkEvent._isRegistered)
				{
					//이벤트가 등록 안되었으면 패스
					continue;
				}

				if((isPressed && hkEvent._isCombination)
					|| (!isPressed && !hkEvent._isCombination))
				{
					//조합키인데 Pressed 이벤트이거나
					//단일키인데 Released 이벤트라면
					//패스
					continue;
				}

				

				//키 입력 체크는 여기서 직접 한다.
				//TODO : 키가 추가되면 여기에 코드를 추가하자
				isValidInput = false;

				switch (hkEvent._reservedHotkey)
				{
					case RESERVED_KEY.Arrow:
						if(
							(keyCode == KeyCode.DownArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow)
							&& !isShift && !isAlt && !isCtrl
							)
						{
							//방향키 + 특수키 없음
							isValidInput = true;
						}
						break;

					case RESERVED_KEY.Arrow_Ctrl:
						if(
							(keyCode == KeyCode.DownArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow)
							&& !isShift && !isAlt && isCtrl
							)
						{
							//방향키 + Ctrl
							isValidInput = true;
						}
						break;

					case RESERVED_KEY.Arrow_Shift:
						if(
							(keyCode == KeyCode.DownArrow || keyCode == KeyCode.UpArrow || keyCode == KeyCode.LeftArrow || keyCode == KeyCode.RightArrow)
							&& isShift && !isAlt && !isCtrl
							)
						{
							//방향키 + Shift
							isValidInput = true;
						}
						break;

					case RESERVED_KEY.EnterOrEscape:
						if(keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter || keyCode == KeyCode.Escape)
						{
							//Enter or Escape
							isValidInput = true;
						}
						break;
						
					case RESERVED_KEY.Escape:
						if(keyCode == KeyCode.Escape)
						{
							//Esc
							isValidInput = true;
						}
						break;

					case RESERVED_KEY.Enter:
						if(keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter)
						{
							//Enter
							isValidInput = true;
						}
						break;

					case RESERVED_KEY.None:
						//>> 이건 아무 처리도 안한다.
						break;
				}

				if(isValidInput)
				{
					//저장된 이벤트를 실행하자
					try
					{
						hkEvent._funcEvent(keyCode, isShift, isAlt, isCtrl, hkEvent._paramObject);

						return hkEvent;
					}
					catch(Exception ex)
					{
						Debug.LogError("AnyPortrait : HotKey Event Exception : " + ex);
						return null;
					}
				}
			}
			return null;
		}


		// Get / Set
		//---------------------------------------------
		public apStringWrapper GetText(HotKeyEvent hotkeyEvent)
		{
			return _labels[hotkeyEvent._labelType];
		}
	}

}