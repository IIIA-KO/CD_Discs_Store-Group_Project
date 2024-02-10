import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminFilmsDelete = () => {
    let { id } = useParams();

    const [items, setItems] = useState([]);
    const [message, setMessage] = useState('');

    let handleDelete = async (e) => {
        try {
            let url = "https://localhost:7117/Film/Delete?" + id;
            console.log(url);
            let res = await fetch(url, {
                method: "DELETE"
            });
            let resJson = await res.json();
            if (res.status === 200) {

                setMessage("Film deleted successfully");
            } else {
                setMessage("Some error occured");
            }
        } catch (err) {
            console.log(err);
        }
    }

    useEffect(() => {
        console.log(id);
        let url = "https://localhost:7117/Film/GetFilm?" + id;
        console.log(url);
        fetch(url).then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
    }, [id])

    return (
        <>
            <AdminPanelHeader />
            <div className='adminfilms'>
                <h1>Are you sure you want to delete this film?</h1>
                <table className='table'>
                    <thead>
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Name</th>
                            <th scope="col">Genre</th>
                            <th scope="col">Producer</th>
                            <th scope="col">Main Role</th>
                            <th scope="col">Age Limit</th>
                            <th scope="col">Cover Image</th>
                            <th scope="col">Image name</th>
                        </tr>
                    </thead>
                    <tbody>
                            <tr key={items.id}>
                                <th scope="row">{items.id}</th>
                                <td>{items.name}</td>
                                <td>{items.genre}</td>
                                <td>{items.producer}</td>
                                <td>{items.mainRole}</td>
                                <td>{items.ageLimit}</td>
                                <td>{items.coverImagePath}</td>
                                <td>{items.imageStorageName}</td>
                            </tr>
                    </tbody>
                </table>
                <button className='delete' onClick={() => handleDelete(items.id)}>Yes, delete it</button>
                <button className='backbutton' onClick={() => window.location.href = "/adminpanel/films"}>Back</button>

                <div className="message">{message ? <p>{message}</p> : null}</div>

            </div>
        </>
    )
}

export default AdminFilmsDelete
