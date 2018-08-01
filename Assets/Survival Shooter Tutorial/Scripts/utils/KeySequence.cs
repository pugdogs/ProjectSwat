using UnityEngine;
using System.Collections;

[ System.Serializable ]
public class KeySequence
{
	public 		KeyCode[]	sequence;
	private		int 		_sequenceIndex;
	private		bool 		_isDetected;

	public bool IsDetected() { return _isDetected; }

	public KeySequence() { sequence = new KeyCode[ 0 ]; }

	public KeySequence( KeyCode[] seq ) {
		sequence = new KeyCode[ seq.Length ];
		seq.CopyTo( sequence, 0 );
	}

	public bool Check()
	{
		if( sequence.Length > 0 )
		{
			if( Input.GetKeyDown( sequence[ _sequenceIndex ] ) )
			{ _sequenceIndex ++; }
			else if( Input.anyKeyDown )
			{ _sequenceIndex = 0; }

			_isDetected = ( _sequenceIndex == sequence.Length ) ? true : false;

			if( _isDetected ) { _sequenceIndex = 0; }
		}
		else { _isDetected = false; }

		return _isDetected;
	}
}