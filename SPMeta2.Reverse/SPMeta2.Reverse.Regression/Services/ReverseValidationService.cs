using SPMeta2.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes.Identity;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Definitions;
using SPMeta2.Reverse.Services;
using SPMeta2.Containers.Extensions;

namespace SPMeta2.Reverse.Regression.Services
{
    public class ReverseValidationService : ModelServiceBase
    {
        #region constructors

        public ReverseValidationService()
        {
            RegisterModelHandlers<ReverseDefinitionValidatorBase>(this, GetType().Assembly);
            ModelIdService = new ReverseModelIdentityService();
        }

        #endregion

        #region properties

        //private MD5HashCodeServiceBase ModelIdService { get; set; }
        private ReverseModelIdentityService ModelIdService { get; set; }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            var validationModelHost = modelHost as ReverseValidationModeHost;

            if (validationModelHost == null)
                throw new ArgumentException("modelHost should be of type ReverseValidationModeHost");

            var orginalModel = validationModelHost.OriginalModel;
            var reversedModel = validationModelHost.ReversedModel;

            var allOriginalNodes = orginalModel.Flatten();
            var allReversedNodes = reversedModel.Flatten();

            foreach (var originalNode in allOriginalNodes)
            {
                if (!originalNode.Options.RequireSelfProcessing)
                    continue;

                if (originalNode.RegIsExcludedFromValidation())
                    continue;

                var originalDefinition = originalNode.Value;

                var originalDefinitionId = ModelIdService.GetDefinitionIdentityKey(originalDefinition);
                var reversedNode = allReversedNodes
                                    .FirstOrDefault(n => ModelIdService.GetDefinitionIdentityKey(n.Value) == originalDefinitionId);

                if (reversedNode == null)
                {
                    // check the level
                    var level = 0;
                    var node = originalNode;

                    while (node != null)
                    {
                        node = allOriginalNodes.FirstOrDefault(n => n.ChildModels.Contains(node));
                        level++;
                    }

                    var defOptions = validationModelHost.ReverseOptions;

                    if (defOptions != null)
                    {
                        var definitionType = originalNode.Value.GetType();
                        var depthOption = defOptions
                                            .Options
                                            .FirstOrDefault(o => o.DefinitionClassFullName == definitionType.FullName
                                            && o is ReverseDepthOption) as ReverseDepthOption;

                        // does it exist? no more that suggested depth
                        if (depthOption != null)
                        {
                            if (depthOption.Depth < level)
                            {

                                // all good, we don't need to validate def which out of the depth leve;
                                originalNode.RegExcludeFromValidation();
                                continue;
                            }
                        }
                    }

                    throw new SPMeta2ReverseException(
                      string.Format("Cannot find node of type:[{0}] by identity id:[{1}]. Original definition is:[{2}]",
                          originalDefinition.GetType(), originalDefinitionId, originalDefinition));
                }

                var definitionValidator = ResolveModelHandlerForNode(originalNode);

                definitionValidator.DeployModel(new ReverseValidationModeHost
                {
                    OriginalModel = originalNode,
                    ReversedModel = reversedNode,
                    ReverseOptions = validationModelHost.ReverseOptions
                }, null);
            }
        }

        #endregion
    }
}
