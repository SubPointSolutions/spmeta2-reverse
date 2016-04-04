using SPMeta2.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Exceptions;
using SPMeta2.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Reverse.Exceptions;
using SPMeta2.Reverse.Regression.Base;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Reverse.Regression.Services
{
  



    public class ReverseValidationService : ModelServiceBase
    {
        #region constructors

        public ReverseValidationService()
        {
            RegisterModelHandlers<ReverseDefinitionValidatorBase>(this, GetType().Assembly);
            ModelIdService = new MD5HashCodeServiceBase();
        }

        #endregion

        #region properties

        private MD5HashCodeServiceBase ModelIdService { get; set; }

        #endregion

        #region methods

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            var validationModelHost = modelHost as ReverseValidationModeHost;

            if (validationModelHost == null)
                throw new ArgumentException("modelHost should be of type ReverseValidationModeHost");

            var orginalModel = validationModelHost.OriginalModel;

            var allOriginalNodes = orginalModel.Flatten();
            var allReversedNodes = orginalModel.Flatten();

            foreach (var originalNode in allOriginalNodes)
            {
                if (!originalNode.Options.RequireSelfProcessing)
                    continue;

                var originalDefinition = originalNode.Value;

                var originalDefinitionHash = ModelIdService.GetHashCode(originalDefinition);
                var reversedNode = allReversedNodes
                                    .First(n => ModelIdService.GetHashCode(n.Value) == originalDefinitionHash);

                if (reversedNode == null)
                    throw new SPMeta2ReverseException(
                        string.Format("Cannot find node of type:[{0}] by hash:[{1}]. Original definition is:[{2}]",
                            originalDefinition.GetType(), originalDefinitionHash, originalDefinition));

                var definitionValidator = ResolveModelHandlerForNode(originalNode);

                definitionValidator.DeployModel(new ReverseValidationModeHost
                {
                    OriginalModel = originalNode,
                    ReversedModel = reversedNode
                }, null);
            }
        }

        #endregion
    }
}
