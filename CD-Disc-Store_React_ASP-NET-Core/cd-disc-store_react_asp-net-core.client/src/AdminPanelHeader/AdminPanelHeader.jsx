import React from 'react'
import { NavLink } from "react-router-dom"
import './adminpanelheader.css'
const AdminPanelHeader = () => {
    return (
        <>
            <header>
                <div className="wrapper">
                    <div className="navbar">
                        <div className="logo"><p><a href="/" className='link'>AdminPanel</a></p></div>
                        <nav>
                            <ul>
                                <li><NavLink to="/adminpanel/disks" className='link' activeClassName="active">Disks</NavLink></li>
                                <li><NavLink to="/adminpanel/films" className='link' activeClassName="active">Films</NavLink></li>
                                <li><NavLink to="/adminpanel/music" className='link' activeClassName="active">Music</NavLink></li>

                            </ul>
                        </nav>
                    </div>
                </div>
            </header>
        </>
    )
}

export default AdminPanelHeader
