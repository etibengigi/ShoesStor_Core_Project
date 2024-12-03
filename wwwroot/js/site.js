const uriShoes = "https://localhost:5224/MyShoes";
const uriUsers = "https://localhost:5224/User";
// const uri = '/Shoes';
let Shoess = [];

function checkToken() {
    fetch(uriShoes, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(getItems())
        .catch(error => {
            sessionStorage.setItem("check", error)
            console.log(error);
            location.href = "./login.html"

        });

}


function addItem() {
    const addNameTextbox = document.getElementById('add-name');      
    const addcolorTextbox = document.getElementById('add-color');                                                  
    const addIsStationaryTextbox = document.getElementById('add-IsStationary');                                                  
                                            
    const item = {
        Id:0,
        UserId:localStorage.getItem("userId"),
        IsStationary: addIsStationaryTextbox.checked,
        companyName: addNameTextbox.value.trim(),
        color:addcolorTextbox.value.trim()
    };

    fetch(uriShoes, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addcolorTextbox.value='';
            addIsStationaryTextbox.checked=false;
        })
        .catch(error => console.error('Unable to add item.', error));
}


function getItems() {
    fetch(uriShoes, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(data => {
            console.log(localStorage.getItem("token"));
            _displayItems(data)
        })
        .catch(error => {
            console.log(error);
            localStorage.setItem("error",error)
            // location.href = "./login.html"

        });

}

function deleteItem(id) {
    fetch(`${uriShoes}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = Shoess.find(item => item.id === id);

    document.getElementById('edit-name').value = item.companyName;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-IsStationary').checked = item.IsStationary;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        Id: parseInt(itemId, 10),
        IsStationary: document.getElementById('edit-IsStationary').checked,
        companyName: document.getElementById('edit-name').value.trim(),
        color:document.getElementById('edit-color').value.trim()
    };

    fetch(`${uriShoes}/${itemId}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem("token")}`

            },
            body: JSON.stringify(item)
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput('editForm');

    return false;
}


function closeInput(formToClose) {
    document.getElementById(formToClose).style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'Shoes' : 'Shoes kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('Shoeses');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isStationaryCheckbox = document.createElement('input');
        isStationaryCheckbox.type = 'checkbox';
        isStationaryCheckbox.disabled = true;
        isStationaryCheckbox.checked = item.isStationary;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isStationaryCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.companyName);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        let textNode1 = document.createTextNode(item.color);
        td3.appendChild(textNode1);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    Shoess = data;
}

if (localStorage.getItem("token") == null) {
    console.log("login");
    sessionStorage.setItem("not", "not exist token")

    location.href = "./login.html"

}
function createLink() {
    if (localStorage.getItem("link") == "true") {

        let link = document.createElement("a");
        link.href = "./userList.html";
        link.innerHTML = "users";
        console.log(sessionStorage.getItem("link"));
        document.body.appendChild(link);
    }
}
console.log(localStorage.getItem("token"));


function editUser() {

    document.getElementById('edit-name-user').value = user.username
    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password-user').value = user.password;
    document.getElementById('editUserForm').style.display = 'block';
    console.log(document.getElementById('edit-id-user').value);
}
function updateUser() {
    const newUser = {
        id: user.id,
        Username: document.getElementById('edit-name-user').value.trim(),
        password: document.getElementById('edit-password-user').value.trim()
    };
    fetch(`${uriUsers}/${user.id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
        body: JSON.stringify(newUser)
    })
        .then(() =>{
            user=newUser;
            userName.innerHTML=user.Username;
        }
        )
        .catch(error => console.error('Unable to update item.', error));

closeInput('editUserForm')
    return false;
}
function createUser(response) {
    user = response;
    userName.innerHTML = user.username;

}
function getUser() {
    const userId = localStorage.getItem("userId");
    fetch(`${uriUsers}/${userId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem("token")}`
        },
    })
        .then(response => response.json())
        .then(response => createUser(response))
        .catch(error =>
            console.log(error));

}

let user;
getUser()
getItems();
createLink()

