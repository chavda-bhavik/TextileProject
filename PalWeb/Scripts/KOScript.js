var JobworkParties = []

var UserViewModel = function () {

    //For Jobwork Party Module
    self.Code = ko.observable();
    self.AgencyName = ko.observable();
    self.Address = ko.observable();
    self.OwnerName = ko.observable();
    self.Mobile = ko.observable();
    self.EmailID = ko.observable();
    self.GSTNo = ko.observable();
    self.JobworkParties = JobworkParties;
    self.JobworkParty;
    self.showJobworkPartyModel = function () {
        self.OwnerName = ko.observable();
        self.AgencyName = ko.observable();
        self.Address = ko.observable();
        self.Mobile = ko.observable();
        self.EmailID = ko.observable();
        self.GSTNo = ko.observable();
        self.Code = ko.observable();

        $("#OwnerName").val("");
        $("#AgencyName").val("");
        $("#Address").val("");
        $("#Mobile").val("");
        $("#Email").val("");
        $("#GSTNo").val("");
        $("#Code").val("");
        $("#JobworkPartySubmitBtn").text("Create");
        $("#JobworkPartyModel").modal('show');
    }
    self.EditJobworkParty = function (JobworkParty) {
        self.JobworkParty = JobworkParty;
        self.OwnerName = ko.observable(JobworkParty.OwnerName);
        self.AgencyName = ko.observable(JobworkParty.AgencyName);
        self.Address = ko.observable(JobworkParty.Address);
        self.Mobile = ko.observable(JobworkParty.Mobile);
        self.EmailID = ko.observable(JobworkParty.EmailID);
        self.GSTNo = ko.observable(JobworkParty.GSTNo);
        self.Code = ko.observable(JobworkParty.Code);

        $("#OwnerName").val(JobworkParty.OwnerName);
        $("#AgencyName").val(JobworkParty.AgencyName);
        $("#Address").val(JobworkParty.Address);
        $("#Mobile").val(JobworkParty.Mobile);
        $("#Email").val(JobworkParty.EmailID);
        $("#GSTNo").val(JobworkParty.GSTNo);
        $("#Code").val(JobworkParty.Code);
        $("#JobworkPartySubmitBtn").text("Update");
        $("#JobworkPartyModel").modal('show');
        //console.log(self.Address());
    }
    self.DeleteJobworkParty = function (JobworkParty) {
        self.JobworkParties.remove(JobworkParty);
        $.ajax({
            url: '/JobworkParty/DeleteJobworkParty',
            type: 'POST',
            contentType: 'application/json;charset=utf-8',
            data: JSON.stringify(JobworkParty),
            success: function (response) {
                self.JobworkParties.remove(JobworkParty);
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    self.CreateJobworkParty = function () {
        let url = "";
        let data = {};
        if ($("#JobworkPartySubmitBtn").text() == "Update") {
            data.Code = self.Code();
            self.JobworkParties.remove(self.JobworkParty);
            url = '/JobworkParty/UpdateJobworkParty'
        } else {
            url = '/JobworkParty/AddJobworkParty'
        }
        data.OwnerName = self.OwnerName();
        data.AgencyName = self.AgencyName();
        data.Address = self.Address();
        data.Mobile = self.Mobile();
        data.EmailID = self.EmailID();
        data.GSTNo = self.GSTNo();
        data.ObjectState = 0;

        $.ajax({
            url: url,
            type: 'POST',
            contentType: 'application/json;charset=utf-8',
            //data: ko.toJSON({data: JobworkParty}),
            data: JSON.stringify(data),
            success: function (response) {
                self.JobworkParties.push(response);
                $("#JobworkPartyModel").modal('hide');
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    //For Jobwork Module
    self.PartyNo = ko.observable();
    self.PartyName = ko.observable();
    self.PhoneNumber = ko.observable();

    self.changed = function () {
        var pno = self.PartyNo();
        for (var i = 0; i < self.fetchJobworkParties().length; i++) {
            let id = self.fetchJobworkParties()[i],
        }
    }
    
}
var JobworkModel = function () {
    self.JobworkParties = JobworkParties
    self.PartyNo = ko.observable()
}

const fetchJobworkParties = () => {
    $.getJSON('/JobworkParty/GetJobworkParties', function (data) {
        JobworkParties = ko.observableArray(data);
        var UsersView = new UserViewModel();
        ko.applyBindings(UsersView);
        
    });
}
const generatePurchaseCode = () => {
    let code = "LOT1234"
}
$(document).ready(function () {
    fetchJobworkParties()
})