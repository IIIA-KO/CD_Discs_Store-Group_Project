import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader';

const AdminDisksDelete = () => {
    let { id } = useParams();

    const [items, setItems] = useState([]);
    const [message, setMessage] = useState('');

    let handleDelete = async (e) => {
        try {
            let url = "https://localhost:7117/Discs/Delete?" + id;
            console.log(url);
            let res = await fetch(url, {
                method: "DELETE"
            });
            let resJson = await res.json();
            if (res.status === 200) {

                setMessage("Disk deleted successfully");
            } else {
                setMessage("Some error occured");
            }
        } catch (err) {
            console.log(err);
        }
    }

    useEffect(() => {
        console.log(id);
        let url = "https://localhost:7117/Discs/GetDisc?" + id;
        fetch(url).then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
    }, [id])

    return (
        <>
            <AdminPanelHeader />
            <div className='admindisks'>
                <h1>Are you sure you want to delete this disk?</h1>
                <table className='table'>
                    <thead>
                        <tr>
                            <th scope="col">ID</th>
                            <th scope="col">Name</th>
                            <th scope="col">Price</th>
                            <th scope="col">Left on stock</th>
                            <th scope="col">Rating</th>
                            <th scope="col">Cover image</th>
                            <th scope="col">Image name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr key={items.id}>
                            <th scope="row">{items.id}</th>
                            <td>{items.name}</td>
                            <td>{items.price}</td>
                            <td>{items.leftOnStock}</td>
                            <td>{items.rating}</td>
                            <td>{items.coverImagePath}</td>
                            <td>{items.imageStorageName}</td>
                        </tr>
                    </tbody>
                </table>
                <button className='delete' onClick={() => handleDelete(items.id)}>Yes, delete it</button>
                <button className='backbutton' onClick={() => window.location.href = "/adminpanel/disks"}>Back</button>

                <div className="message">{message ? <p>{message}</p> : null}</div>

            </div>
        </>
    )
}

export default AdminDisksDelete
