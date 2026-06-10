const form = document.getElementById("initialLogin");

form.addEventListener("submit", async (event) => {
    event.preventDefault();
    const username = document.getElementById("name").value;
    const password = document.getElementById("password").value;

    try {
        await fetch("http://localhost:5169/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                Username: username,
                Password: password
            })
        }) 
        .then (response => {
            if (response.ok) {
                window.location.href = "../home/home.html"
                return response.json();
            } 
            else {
                alert(response.status)
            }
        })
    }
    catch (error) {
        console.log(error);
        alert("Não deu certo");
        alert(error)
    }
});