App = Em.Application.create();

App.Router.map(function () {
    this.resource('colors', function () { });
});

App.ColorsRoute = Em.Route.extend({
    model: function () {
        return ['red', 'yellow', 'blue'];
    }
});