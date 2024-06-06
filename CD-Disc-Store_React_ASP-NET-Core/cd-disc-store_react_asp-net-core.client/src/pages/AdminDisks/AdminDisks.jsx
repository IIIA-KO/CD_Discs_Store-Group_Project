import React from 'react'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import AdminPanelHeader from '../../AdminPanelHeader/AdminPanelHeader'
import './admindisks.css';


const AdminDisks = () => {
    const [items, setItems] = useState([]);
    const [currentPage, setCurrentPage] = useState(0);
    let navigate = useNavigate();
    function handlePagination(pageNumber, itemsPerPage) {
        console.log(pageNumber);
        console.log(itemsPerPage);
        if (pageNumber)
        if (pageNumber < 0) {
            handlePagination(pageNumber+1, itemsPerPage);
            document.getElementById("leftarrow").disabled = true;
            return;
        } else{
            document.getElementById("leftarrow").disabled = false;
        }
        let fetchurl = "https://localhost:7117/Discs/GetAll?skip=" + pageNumber * itemsPerPage;
        fetch(fetchurl)
        .then(res => res.json())
        .then(data => {
            if (data.items.length == 0&&pageNumber>0) {
                handlePagination(pageNumber-1, itemsPerPage);
                document.getElementById("rightarrow").disabled = true;
            } else if (data.items.length>0){
                document.getElementById("rightarrow").disabled = false;
            }
            if(data.items.length>itemsPerPage){
                data.items.slice(0, itemsPerPage);
            }
            setItems(data.items);
        })
        .catch(error => console.error(error));
        setCurrentPage(pageNumber);
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
                                <td>{item.leftOnStock}</td>
                                <td>{item.rating}</td>
                                <td>{item.coverImagePath}</td>
                                <td>{item.imageStorageName}</td>
                                <td><button className='edit' onClick={() => window.location.href = "/adminpanel/disks/edit/id=" + item.id}>Edit</button></td>
                                <td><button className='delete' onClick={() => window.location.href = "/adminpanel/disks/delete/id=" + item.id}>Delete</button></td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <div className='pagination'>
                    <button id="leftarrow" onClick={() => handlePagination(currentPage - 1, 20)}>&lt;</button>
                    <button id="rightarrow" onClick={() => handlePagination(currentPage + 1, 20)}>&gt;</button>
                </div>
            </div>
        </>
    )
}

export default AdminDisks
