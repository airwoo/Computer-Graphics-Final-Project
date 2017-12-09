using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;

namespace TreeSharpPlus
{
	/// <summary>
	/// Evaluates a lambda function. Returns RunStatus.Success if the lambda
	/// evaluates to false. Returns RunStatus.Failure if it evaluates to true.
	/// </summary>
	public class LeafInvert : Node
	{
		protected Func<bool> func_assert = null;

		public LeafInvert(Func<bool> assertion)
		{
			this.func_assert = assertion;
		}

		public override IEnumerable<RunStatus> Execute()
		{
			if (this.func_assert != null)
			{
				bool result = this.func_assert.Invoke();
				//Debug.Log(result);
				if (result == true)
					yield return RunStatus.Failure;
				else
					yield return RunStatus.Success;
				yield break;
			}
			else
			{
				throw new ApplicationException(this + ": No method given");
			}
		}
	}
}