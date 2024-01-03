var Base64Command = (function (_super) {
    __extends(Base64Command, _super);
    function Base64Command() {
        return _super !== null && _super.apply(this, arguments) || this;
    }

    Base64Command.prototype.execute = function () {
        var params = this.CommandParam;

        var op = this.evaluateFormula(params.Operation);
        var encoder = this.evaluateFormula(params.Encoding);
        var input = this.evaluateFormula(params.InputString);
        var resultProp = params.ResultTo;

        var result = input;
        if (op === SupportedOperations.Encrypt) {
            if (encoder === SupportedEncoding.Latin) {
                result = btoa(input);
            } else {
                result = Base64.encode(input);
            }
        } else {
            if (encoder === SupportedEncoding.Latin) {
                result = atob(input);
            } else {
                result = Base64.decode(input);
            }
        }

        if (resultProp && resultProp != "") {
            Forguncy.CommandHelper.setVariableValue(resultProp, result);
        } else {
            console.log("OutParamaterName was not set, the value is: " + JSON.stringify(result));
        }
        
    };

    var SupportedOperations = {
        Encrypt:0,
        Decrypt:1
    };

    var SupportedEncoding = {
        Latin: 0,
        UTF8: 1
    };

    return Base64Command;
}(Forguncy.CommandBase));


Forguncy.CommandFactory.registerCommand("StringEncryptionPlugin.Base64Command, StringEncryptionPlugin", Base64Command);