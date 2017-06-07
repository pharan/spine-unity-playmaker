/******************************************************************************
 * Spine Runtimes Software License v2.5
 *
 * Copyright (c) 2013-2016, Esoteric Software
 * All rights reserved.
 *
 * You are granted a perpetual, non-exclusive, non-sublicensable, and
 * non-transferable license to use, install, execute, and perform the Spine
 * Runtimes software and derivative works solely for personal or internal
 * use. Without the written permission of Esoteric Software (see Section 2 of
 * the Spine Software License Agreement), you may not (a) modify, translate,
 * adapt, or develop new applications using the Spine Runtimes or otherwise
 * create derivative works or improvements of the Spine Runtimes or (b) remove,
 * delete, alter, or obscure any trademarks or any copyright, trademark, patent,
 * or other intellectual property or proprietary rights notices on or in the
 * Software, including any copy thereof. Redistributions in binary or source
 * form must include this license and terms.
 *
 * THIS SOFTWARE IS PROVIDED BY ESOTERIC SOFTWARE "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL ESOTERIC SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES, BUSINESS INTERRUPTION, OR LOSS OF
 * USE, DATA, OR PROFITS) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.PlayMaker;
using HutongGames.PlayMaker;

namespace Spine.Unity.Modules.PlayMaker {

	using Tooltip = HutongGames.PlayMaker.TooltipAttribute;

	[ActionCategory("Spine")]
	public class SpineSkeletonFlipAction : FsmStateAction {

		[RequiredField]
		[CheckForComponent(typeof(ISkeletonComponent))]
		[Tooltip("A target Spine GameObject.")]
		public FsmOwnerDefault spineGameObject;

		public SkeletonAxis2D flipAxis;
		public enum SkeletonAxis2D {
			X,
			Y
		}

		[RequiredField]
		public FsmBool flip;

		ISkeletonComponent component;

		public override void Reset () {
			flipAxis = SkeletonAxis2D.X;
			if (flip != null && !flip.IsNone) flip.Value = false;
		}

		public override void OnEnter () {
			var go = Fsm.GetOwnerDefaultTarget(spineGameObject);
			if (go != null) {
				component = component != null ? component : go.GetComponent<ISkeletonComponent>();
				if (component != null && !flip.IsNone) {
					var skeleton = component.Skeleton;

					switch (flipAxis) {
					case SkeletonAxis2D.X:
						skeleton.flipX = flip.Value;
						break;
					case SkeletonAxis2D.Y:
						skeleton.flipY = flip.Value;
						break;
					}

				} 
			} 
			Finish();
		}
	}
}