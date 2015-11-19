'use strict';
angular.module('todoApp')
.controller('userDataCtrl', ['$scope', 'adalAuthenticationService', function ($scope, adalService) {
    console.log($scope.userInfo);
    console.log(adalService.userInfo);

}]);