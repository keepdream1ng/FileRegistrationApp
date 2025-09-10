using System;

namespace FileRegistrationApp;
public class PathNotFoundException : Exception
{
	public PathNotFoundException(string message) : base(message)
	{
	}
}
