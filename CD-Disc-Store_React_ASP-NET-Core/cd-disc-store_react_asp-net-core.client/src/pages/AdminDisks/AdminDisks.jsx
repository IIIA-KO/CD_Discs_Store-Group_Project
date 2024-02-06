import React from 'react'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader'


const AdminDisks = () => {
    const [items, setItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    let navigate=useNavigate();
    function handlePagination(pageNumber, disksPerPage) {
        if (pageNumber < 1) {
            return;
        }
        setCurrentPage(pageNumber);
        let fetchurl = "https://localhost:7117/Discs/GetAll?skip=" + currentPage * disksPerPage + "&pagesize=" + disksPerPage;
        fetch(fetchurl).then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
    }

    function deleteConfirmation(itemId) {
        navigate('/adminpanel/disks/delete/id=' + itemId, { replace: true });
    }

    useEffect(() => {
        handlePagination(currentPage, 20)
    }, [])

    return (
        <>
            <AdminPanelHeader />
            <div className='admindisks'>
                <h1>Discs</h1>
                <button className='add' onClick={() => window.location.href = "/adminpanel/disks/add"}>Add</button>
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
                            <th scope="col">Edit</th>
                            <th scope="col">Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {items.map(item => (
                            <tr key={item.id}>
                                <th scope="row">{item.id}</th>
                                <td>{item.name}</td>
                                <td>{item.price}</td>
                                <td textAlign='center'>{item.leftOnStock}</td>
                                <td>{item.rating}</td>
                                <td>{item.coverImagePath}</td>
                                <td>{item.imageStorageName}</td>
                                <td><button className='edit'onClick={() => window.location.href="/adminpanel/disks/edit/id=" + item.id}>Edit</button></td>
                                <td><button className='delete' onClick={() => window.location.href="/adminpanel/disks/delete/id=" + item.id}>Delete</button></td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <div className='pagination'>
                    <button onClick={() => handlePagination(currentPage - 1, 20)}>&lt;</button>
                    <button onClick={() => handlePagination(currentPage + 1, 20)}>&gt;</button>
                </div>
            </div>
        </>
    )
}

export default AdminDisks
