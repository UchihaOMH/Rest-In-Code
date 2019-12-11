using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 인터페이스를 상속받는 클래스들은, Input을 받아서 적절한 Output을하는 기능을 수행한다.
/// ex) move스테이트의 경우, 정해진 버튼을 누르면 움직인다
/// </summary>
public interface IPlayerAnimState
{
    void InputProcess();
}