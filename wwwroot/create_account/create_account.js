const form = document.getElementById("formToCreate");

form.addEventListener("submit", async (event) => {
    event.preventDefault();

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    try {
        await fetch ("http://localhost:5169/users", {
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
                return response.json();
            } 
            else {
                alert("Erro: " + response.status);
                throw new Error("Falha na API");
            }
        })
        .then (data => {
            console.log("Conta criada: " + data);
            window.location.href = "../add_favorite_categories/add_favorite_categories.html"
        })
    }
    catch (error) {
        console.error(error);
        alert("Não deu certo");
        alert(error.message)
    }
})