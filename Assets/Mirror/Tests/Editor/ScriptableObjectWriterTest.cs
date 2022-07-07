using NUnit.Framework;
using UnityEngine;

namespace Mirror.Tests
{
	public class MyScriptableObject : ScriptableObject
	{
		public int someData;
	}

	[TestFixture]
	public class ScriptableObjectWriterTest
	{

		// ArraySegment<byte> is a special case,  optimized for no copy and no allocation
		// other types are generated by the weaver


		public struct ScriptableObjectMessage : NetworkMessage
		{
			public MyScriptableObject scriptableObject;
		}

		[Test]
		public void TestWriteScriptableObject()
		{
			ScriptableObjectMessage message = new ScriptableObjectMessage
			{
				scriptableObject = ScriptableObject.CreateInstance<MyScriptableObject>()
			};

			message.scriptableObject.someData = 10;

			byte[] data = MessagePackingTest.PackToByteArray(message);

			ScriptableObjectMessage unpacked = MessagePackingTest.UnpackFromByteArray<ScriptableObjectMessage>(data);

			Assert.That(unpacked.scriptableObject, Is.Not.Null);
			Assert.That(unpacked.scriptableObject.someData, Is.EqualTo(10));
		}

	}
}
