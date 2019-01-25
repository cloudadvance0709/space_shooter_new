using System;
using System.Runtime.InteropServices;
using UnityEngine;

[SerializableAttribute]
[ComVisibleAttribute(true)]
public class SpaceShooterException : Exception
{
		public SpaceShooterException ()
		{

		}		

		public SpaceShooterException (string message) : base(message)
		{

		}

		public SpaceShooterException (string message, Exception innerException) : base(message, innerException)
		{

		}
}


