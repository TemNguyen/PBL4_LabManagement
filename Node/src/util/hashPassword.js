const bcrypt = require('bcrypt');
const saltRounds = 10;
const myPlaintextPassword = 'jThanh8144$PBL4';
const someOtherPlaintextPassword = 'not_bacon';

bcrypt.compare(myPlaintextPassword, hash).then(function(result) {
    // result == true
});
bcrypt.compare(someOtherPlaintextPassword, hash).then(function(result) {
    // result == false
});