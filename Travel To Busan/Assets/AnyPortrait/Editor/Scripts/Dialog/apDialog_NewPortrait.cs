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
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

using AnyPortrait;

namespace AnyPortrait
{

	public class apDialog_NewPortrait : EditorWindow
	{
		// Members
		//------------------------------------------------------------------
		private static apDialog_NewPortrait s_window = null;

		private apEditor _editor = null;
		private object _loadKey = null;

		public delegate void FUNC_NEW_PORTRAIT_RESULT(bool isSuccess, object loadKey, string name);
		private FUNC_NEW_PORTRAIT_RESULT _funcResult = null;

		private string _newPortraitName = "";

		// Show Window
		//------------------------------------------------------------------
		public static object ShowDialog(apEditor editor, FUNC_NEW_PORTRAIT_RESULT funcResult)
		{
			CloseDialog();

			if (editor == null)
			{
				return null;
			}

			EditorWindow curWindow = EditorWindow.GetWindow(typeof(apDialog_NewPortrait), true, "Make New Portrait", true);
			apDialog_NewPortrait curTool = curWindow as apDialog_NewPortrait;

			object loadKey = new object();
			if (curTool != null && curTool != s_window)
			{
				int width = 300;
				int height = 110;
				s_window = curTool;
				s_window.position = new Rect((editor.position.xMin + editor.position.xMax) / 2 - (width / 2),
												(editor.position.yMin + editor.position.yMax) / 2 - (height / 2),
												width, height);

				s_window.Init(editor, loadKey, funcResult);

				return loadKey;
			}
			else
			{
				return null;
			}
		}

		public static void CloseDialog()
		{
			if (s_window != null)
			{
				try
				{
					s_window.Close();
				}
				catch (Exception ex)
				{
					Debug.LogError("Close Exception : " + ex);
				}
				s_window = null;
			}
		}

		// Init
		//------------------------------------------------------------------
		public void Init(apEditor editor, object loadKey, FUNC_NEW_PORTRAIT_RESULT funcResult)
		{
			_editor = editor;
			_loadKey = loadKey;
			_funcResult = funcResult;
			_newPortraitName = "New Portrait";
		}

		// GUI
		//------------------------------------------------------------------
		void OnGUI()
		{
			int width = (int)position.width;
			int height = (int)position.height;
			if (_editor == null || _funcResult == null)
			{
				CloseDialog();
				return;
			}

			//만약 Portriat가 바뀌었거나 Editor가 리셋되면 닫자
			if (_editor != apEditor.CurrentEditor)
			{
				CloseDialog();
				return;
			}

			width -= 10;
			//Bake 설정
			EditorGUILayout.LabelField(_editor.GetText(TEXT.DLG_NewPortraitName), GUILayout.Width(width));//"New Portrait Name"
			//X, Y 개수를 표시

			GUILayout.Space(10);

			_newPortraitName = EditorGUILayout.TextField(_newPortraitName, GUILayout.Width(width));

			GUILayout.Space(20);
			EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
			int width_Btn = ((width - 10) / 2) - 4;
			if (GUILayout.Button(_editor.GetText(TEXT.DLG_MakePortrait), GUILayout.Width(width_Btn), GUILayout.Height(30)))//"Make Portrait"
			{
				_funcResult(true, _loadKey, _newPortraitName);
				CloseDialog();
			}
			if (GUILayout.Button(_editor.GetText(TEXT.DLG_Cancel), GUILayout.Width(width_Btn), GUILayout.Height(30)))//"Cancel"
			{
				_funcResult(false, _loadKey, "");
				CloseDialog();
			}
			EditorGUILayout.EndHorizontal();

		}
	}

}