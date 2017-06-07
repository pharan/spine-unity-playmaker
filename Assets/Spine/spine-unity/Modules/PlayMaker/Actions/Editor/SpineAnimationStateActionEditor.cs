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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using HutongGames.PlayMakerEditor;

using Spine.Unity;
using Spine.Unity.Editor;
using Spine.Unity.Modules.PlayMaker;


namespace Spine.Unity.Editor.PlayMaker {
	
	[CustomActionEditor (typeof(SpineAnimationStateAction))]
	public class SpineAnimationStateActionEditor : CustomActionEditor {
		int animationIndex = 0;
		readonly List<string> animationNames = new List<string>();
		readonly List<GUIContent> popupMenuItems = new List<GUIContent>();
		GUIContent[] menuItems;
		SpineAnimationStateAction action;

		public override void OnEnable () {
			action = target as SpineAnimationStateAction;
		}

		public override bool OnGUI () {
			var isDirty = false;

			EditField("spineGameObject");
			if (action == null || action.Fsm == null)
				return isDirty || GUI.changed;

			GameObject go = action.Fsm.GetOwnerDefaultTarget(action.spineGameObject);

			if (go == null)
				return isDirty || GUI.changed;

			var component = go.GetComponent<IAnimationStateComponent>();
			var skeletonComponent = component as ISkeletonComponent;
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.ObjectField("Spine Component", skeletonComponent as Object, typeof(Component), true);
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.Space();
			EditField("animationStateCall");

			if (action.animationStateCall == SpineAnimationStateAction.AnimationStateCall.SetAnimation ||
				action.animationStateCall == SpineAnimationStateAction.AnimationStateCall.AddAnimation) {

				if (component != null) {
					if (animationNames.Count == 0) {
						SpineAnimationDrawer.GetAnimationMenuItems(skeletonComponent.Skeleton.Data, animationNames, popupMenuItems);
						menuItems = popupMenuItems.ToArray();
					}

					animationIndex = animationNames.IndexOf(action.animationName);
					if (animationIndex == -1) animationIndex = 0;

					animationIndex = EditorGUILayout.Popup(new GUIContent("Animations"), animationIndex, menuItems);
					string selectedAnimationName = animationNames[animationIndex];
					if (action.animationName != selectedAnimationName) {
						action.animationName = selectedAnimationName;
						isDirty = true;
					}
				}

				EditField("loop");
			}

			if (action.animationStateCall == SpineAnimationStateAction.AnimationStateCall.AddAnimation || action.animationStateCall == SpineAnimationStateAction.AnimationStateCall.AddEmptyAnimation)
				EditField("delay");

			return isDirty || GUI.changed;
		}
	}

}
