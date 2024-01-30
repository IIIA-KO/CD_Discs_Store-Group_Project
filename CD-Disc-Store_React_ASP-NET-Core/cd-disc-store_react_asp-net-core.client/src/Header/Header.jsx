import React from 'react'
import { NavLink } from "react-router-dom"
import './header.css'
const Header = () => {
  return (
    <>
      <header>
        <div className="wrapper">
          <div className="header">
            <div className="left_side">
              <div className="logo"><p><a href="/">TheDisk</a></p></div>
              <nav>
                <ul>
                  <li><NavLink to="/" activeClassName="active">Home</NavLink></li>
                  <li><NavLink to="/disks" activeClassName="active">Disks</NavLink></li>
                  <li><NavLink to="/films" activeClassName="active">Films</NavLink></li>
                  <li><NavLink to="/music" activeClassName="active">Music</NavLink></li>
                </ul>
              </nav>
            </div>
            <div className="right_side">
              <div className='CartAndProfile'>
                <button><NavLink to="/Cart" >Cart</NavLink></button>
                <button><NavLink to="/Profile" >Profile</NavLink></button>
              </div>
            </div>
          </div>
        </div>
      </header>
    </>
  )
}

export default Header
